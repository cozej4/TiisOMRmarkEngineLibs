//Sample license text.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GIIS.DataLayer;
using Npgsql;
using MSTestHacks;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Data;
using System.Diagnostics;
using GIIS.DataLayer.Tests.Helpers;

namespace GIIS.DataLayer.Tests
{
    /// <summary>
    /// A test suite for entity CRUD methods.
    /// </summary>
    [TestClass]
    public class EntityCRUDTests : TestBase
    {
        public int ErrorCode { get { return -1; } }

        private static IEnumerable<Type> EntityTypes
        {
            get
            {
                return GetTypesInNamespace(Assembly.GetExecutingAssembly().GetReferencedAssemblies(), "GIIS.DataLayer");
            }
        }

        private static IEnumerable<Type> GetTypesInNamespace(AssemblyName[] assemblyNames, string nameSpace)
        {
            var referencedTypes = assemblyNames
                .SelectMany(x => Assembly.Load(x.FullName).GetTypes());

            return referencedTypes
                .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.Name != "DBManager");
        }

        /// <summary>
        /// Attempts to insert an instantiated unpopulated entity into each table.
        /// It is expected the method will check for missing required fields before executing the insert.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataSource("GIIS.DataLayer.Tests.EntityCRUDTests.EntityTypes")]
        public void Entity_InsertEmpty()
        {
            var methodName = "Insert";
            var entityType = this.TestContext.GetRuntimeDataSourceObject<Type>();
            var insertMethod = entityType.GetMethod(methodName);

            // Check if the type has an insert method.
            if (insertMethod != null)
            {
                // Generate default, unpopulated values for all parameters.
                var methodParams = insertMethod.GetParameters()
                    .Select(x => Activator.CreateInstance(entityType))
                    .ToArray();

                Debug.WriteLine("Invoking method '{0}' on type '{1}'.", methodName, entityType.Name);

                int returnId = (int)insertMethod.Invoke(null, methodParams);
            }
            else
            {
                Assert.Inconclusive("Type '{0}' does not have a '{1}' method.", entityType.FullName, methodName);
            }
        }

        /// <summary>
        /// Attempts to insert a null entity into each table.
        /// It is expected the method will check for a null reference before executing the insert.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataSource("GIIS.DataLayer.Tests.EntityCRUDTests.EntityTypes")]
        public void Entity_InsertNull()
        {
            var methodName = "Insert";
            var entityType = this.TestContext.GetRuntimeDataSourceObject<Type>();
            var insertMethod = entityType.GetMethod(methodName);

            // Check if the type has an insert method.
            if (insertMethod != null)
            {
                // Generate default values for all parameters.
                var methodParams = insertMethod.GetParameters()
                    .Select(x => x.ParameterType.GetDefaultValue())
                    .ToArray();

                Debug.WriteLine("Invoking method '{0}' on type '{1}'.", methodName, entityType.Name);

                insertMethod.Invoke(null, methodParams);
            }
            else
            {
                Assert.Inconclusive("Type '{0}' does not have a '{1}' method.", entityType.FullName, methodName);
            }
        }

        /// <summary>
        /// Attempts to update a null entity in each table.
        /// It is expected the method will check for a null reference before executing the update.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataSource("GIIS.DataLayer.Tests.EntityCRUDTests.EntityTypes")]
        public void Entity_UpdateNull()
        {
            var methodName = "Update";
            var entityType = this.TestContext.GetRuntimeDataSourceObject<Type>();
            var updateMethod = entityType.GetMethod(methodName);

            // Check if the type has an insert method.
            if (updateMethod != null)
            {
                // Generate default values for all parameters.
                var methodParams = updateMethod.GetParameters()
                    .Select(x => x.ParameterType.GetDefaultValue())
                    .ToArray();

                Debug.WriteLine("Invoking method '{0}' on type '{1}'.", methodName, entityType.Name);

                updateMethod.Invoke(null, methodParams);
            }
            else
            {
                Assert.Inconclusive("Type {0} does not have an '{1}' method.", entityType.FullName, methodName);
            }
        }

        /// <summary>
        /// Attempts to delete an entity from each table using default values for parameters.
        /// It is expected the method will check for invalid arguments before attempting to perform the delete.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataSource("GIIS.DataLayer.Tests.EntityCRUDTests.EntityTypes")]
        public void Entity_DeleteInvalidId()
        {
            var methodName = "Delete";
            var entityType = this.TestContext.GetRuntimeDataSourceObject<Type>();
            var deleteMethod = entityType.GetMethod(methodName);

            // Check if the type has a delete method.
            if (deleteMethod != null)
            {
                // Generate default values for all parameters.
                var methodParams = deleteMethod.GetParameters()
                    .Select(x => x.ParameterType.GetDefaultValue())
                    .ToArray();

                Debug.WriteLine("Invoking method '{0}' on type '{0}'.", methodName, entityType.Name);

                // It is expected this will throw an exception.
                deleteMethod.Invoke(null, methodParams);
            }
            else
            {
                Assert.Inconclusive("Type '{0}' does not have a '{1}' method.", entityType.FullName, methodName);
            }
        }

        //  [TestMethod]
        public void Testtest()
        {
            bool dbExists;

            using (NpgsqlConnection conn = new NpgsqlConnection(DBManager.ConnectionString))
            {
                conn.Open();
                string cmdText = "SELECT 1 FROM pg_database WHERE datname='aws'";

                using (NpgsqlCommand cmd = new NpgsqlCommand(cmdText, conn))
                {
                    dbExists = cmd.ExecuteScalar() != null;
                }


                if (!dbExists)
                {
                    var createCmd = new NpgsqlCommand(@"
                    CREATE DATABASE IF NOT EXISTS aws
                    WITH OWNER = postgres
                    ENCODING = 'UTF8'
                    CONNECTION LIMIT = -1;
                    ", conn);

                    createCmd.ExecuteNonQuery();
                }

            }

            Assert.IsTrue(dbExists);
        }
    }
}
