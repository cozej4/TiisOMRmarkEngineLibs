<?xml version="1.0" encoding="utf-8"?>
<!--
 -	TIIS HIE Synchronization Program, Copyright (C) 2015 ecGroup
 -  Development services by Fyfe Software Inc.
 - 
 -    Licensed under the Apache License, Version 2.0 (the "License");
 -    you may not use this file except in compliance with the License.
 -    You may obtain a copy of the License at
 -
 -        http://www.apache.org/licenses/LICENSE-2.0
 -
 -    Unless required by applicable law or agreed to in writing, software
 -    distributed under the License is distributed on an "AS IS" BASIS,
 -    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 -    See the License for the specific language governing permissions and
 -    limitations under the License.
 -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:csd="urn:ihe:iti:csd:2013"
                xmlns:idea="urn:functions"
>
    <xsl:output method="text" indent="yes"/>

  <msxsl:script language="c#" implements-prefix="idea">
    public String safe(String i)
    {
    return i.Replace("'","''");
    }
    public bool startsWith(String start, String node)
    {
      return node.StartsWith(start);
    }
  </msxsl:script>
  
  <!-- CSD Instance file to GIIS -->
  <xsl:template match="/csd:CSD">
    <xsl:apply-templates select="csd:organizationDirectory"/>
  </xsl:template>
  <xsl:template match="csd:organizationDirectory">
    <xsl:apply-templates select="csd:organization"/>  
  </xsl:template>
  <xsl:template match="csd:organization">
    <xsl:if test="idea:startsWith('TZ.NT.AS', csd:otherID/@code) or idea:startsWith('urn:uuid:EDFD5B83-560D-39EB-9121-D8EB33326F1D', csd:parent/@entityID)">
      <!-- Insert into the PLACE table -->
      <xsl:choose>
        <xsl:when test="csd:parent">

          <xsl:variable name="parentId" select="csd:parent/@entityID"/>
          <!--
          INSERT INTO "HEALTH_FACILITY" ("ID", "NAME", "CODE", "PARENT_ID", "LEAF", "MODIFIED_ON", "MODIFIED_BY", "VACCINATION_POINT", "VACCINE_STORE", "TYPE_ID")
          SELECT nextval('"HEALTH_FACILITY_ID_seq"'), '<xsl:value-of select="idea:safe(csd:primaryName/text())"/>', '<xsl:value-of select="idea:safe(@entityID)"/>', "ID", <xsl:value-of select="csd:codedType/@code = 6"/>, CURRENT_TIMESTAMP, 1, <xsl:value-of select="csd:codedType/@code = 6"/>, true,
          (SELECT "ID" FROM "HEALTH_FACILITY_TYPE" WHERE "CODE" = '<xsl:value-of select="csd:codedType/@code"/>')
          FROM "HEALTH_FACILITY" WHERE "CODE" = '<xsl:value-of select="idea:safe($parentId)"/>';-->

          INSERT INTO "PLACE" ("ID", "NAME", "PARENT_ID", "LEAF", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY", "HEALTH_FACILITY_ID", "CODE")
          SELECT nextval('"PLACE_ID_seq"'), '<xsl:value-of select="idea:safe(csd:primaryName/text())"/>', "ID", <xsl:value-of select="csd:codedType/@code = 6"/>, TRUE, CURRENT_TIMESTAMP, 1,
          (SELECT "ID" FROM "HEALTH_FACILITY" WHERE "CODE" = '<xsl:value-of select="@entityID"/>'),
          '<xsl:value-of select="idea:safe(csd:otherID/@code)"/>'
          FROM "PLACE" WHERE "CODE" = '<xsl:value-of select="idea:safe(../csd:organization[@entityID = $parentId]/csd:otherID/@code)"/>';
        </xsl:when>
        <xsl:otherwise>

          <!--INSERT INTO "HEALTH_FACILITY" ("ID", "NAME", "CODE", "PARENT_ID", "LEAF", "MODIFIED_ON", "MODIFIED_BY", "VACCINATION_POINT", "VACCINE_STORE", "TYPE_ID")
          VALUES (nextval('"HEALTH_FACILITY_ID_seq"'), '<xsl:value-of select="idea:safe(csd:primaryName/text())"/>', '<xsl:value-of select="idea:safe(@entityID)"/>', 0, <xsl:value-of select="csd:codedType/@code = 6"/>, CURRENT_TIMESTAMP, 1, <xsl:value-of select="csd:codedType/@code = 6"/>, true,
          (SELECT "ID" FROM "HEALTH_FACILITY_TYPE" WHERE "CODE" = '<xsl:value-of select="csd:codedType/@code"/>'));-->

          INSERT INTO "PLACE" ("ID", "NAME", "PARENT_ID", "LEAF", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY", "HEALTH_FACILITY_ID", "CODE")
          VALUES(nextval('"PLACE_ID_seq"'), '<xsl:value-of select="idea:safe(csd:primaryName/text())"/>', 0, <xsl:value-of select="csd:codedType/@code = 6"/>, TRUE, CURRENT_TIMESTAMP, 1,
          (SELECT "ID" FROM "HEALTH_FACILITY" WHERE "CODE" = '<xsl:value-of select="idea:safe(@entityID)"/>'),
          '<xsl:value-of select="idea:safe(csd:otherID/@code)"/>');

        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <xsl:template match="@* | node()">
  </xsl:template>
  
  
</xsl:stylesheet>
