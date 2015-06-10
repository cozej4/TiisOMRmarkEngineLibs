using GIIS.BusinessLogic.Exceptions;
using GIIS.DataLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.BusinessLogic
{
    /// <summary>
    /// Vaccination logic
    /// </summary>
    public class VaccinationLogic
    {
        /// <summary>
        /// Recalculates scheduled date for vaccination event following <paramref name="ve"/> with the difference between scheduled date and vaccination date
        /// </summary>
        /// <param name="ve">The vaccination event that triggers the reschedule of the following vaccination event </param>
        /// <param name="daysdiff">The difference between scheduled date and actual vaccination date</param>
        /// <param name="remove"> If the vaccination event is being rolled back so rescheduling needs to be rolled back also</param>
        /// <returns></returns>
        public VaccinationEvent RescheduleNextDose(VaccinationEvent ve, bool remove)
        {
            if (ve == null)
                throw new ArgumentNullException("ve");

            VaccinationEvent vaccevent = new VaccinationEvent();
            int currAgeDef = ve.Dose.AgeDefinition.Days;
            int daysdiff = 0;

            int dosenum = ve.Dose.DoseNumber + 1;
            if (dosenum > 1)
            {
                int vid = VaccinationEvent.NextDose(ve.Dose.ScheduledVaccinationId, ve.ChildId, dosenum);
                if (vid != -1)
                {
                    vaccevent = VaccinationEvent.GetVaccinationEventById(vid);
                    int nextAgeDef = vaccevent.Dose.AgeDefinition.Days;
                    daysdiff = nextAgeDef - currAgeDef;

                    if (remove)
                    {
                        VaccinationEvent.UpdateIsActive(vid, false);
                        VaccinationEvent.UpdateEvent(vid, ve.ScheduledDate.AddDays(daysdiff));
                    }
                    else
                    {
                        VaccinationEvent.UpdateIsActive(vid, true);
                        VaccinationEvent.UpdateEvent(vid, ve.VaccinationDate.AddDays(daysdiff));

                        //update others if the vaccination date is later
                        List<VaccinationEvent> vacceventlist = VaccinationEvent.GetVaccinationEventByAppointmentId(ve.AppointmentId);
                        foreach (VaccinationEvent v in vacceventlist)
                        {
                            if (ve.Id != v.Id)
                            {
                                if (v.VaccinationStatus || v.NonvaccinationReasonId != 0)
                                {
                                    if (ve.VaccinationDate < v.VaccinationDate)
                                    {
                                        VaccinationEvent nv = VaccinationEvent.GetVaccinationEventById(VaccinationEvent.NextDose(ve.Dose.ScheduledVaccinationId, ve.ChildId, dosenum));
                                        VaccinationEvent vn = VaccinationEvent.GetVaccinationEventById(vid);
                                        VaccinationEvent.UpdateEvent(nv.Id, vn.ScheduledDate);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return vaccevent;
        }

        /// <summary>
        /// Updates Vaccination Event with given id <paramref name="id"/> with all the other parameters.
        /// </summary>
        /// <param name="id">The id of the vaccination event that is being updated </param>
        /// <param name="lotId">The vaccine lot used for this vaccination event</param>
        /// <param name="vaccinationdate">The vaccination date for this vaccination event</param>
        /// <param name="hfId">The health facility where this vaccination event happened</param>
        /// <param name="done">Status of the vaccination event</param>
        /// <param name="nonvaccreasonId">The reason for not vaccinating for this vaccination event</param>
        /// <returns></returns>
        public VaccinationEvent UpdateVaccinationEvent(int id, int lotId, DateTime vaccinationdate, int hfId, bool done, int nonvaccreasonId, int userId)
        {
            if (id <= 0)
                throw new ArgumentException("id");
            if (hfId <= 0)
                throw new ArgumentException("hfId");
            if (userId <= 0)
                throw new ArgumentException("userId");
            
            VaccinationEvent o = VaccinationEvent.GetVaccinationEventById(id);
            o.VaccineLotId = lotId;

            o.VaccinationDate = vaccinationdate;
            o.HealthFacilityId = hfId;
            o.VaccinationStatus = done;
            if (done)
                o.NonvaccinationReasonId = 0;
            else
            {
                o.NonvaccinationReasonId = nonvaccreasonId;
                o.VaccineLotId = 0;
            }
            o.ModifiedOn = DateTime.Now;
            o.ModifiedBy = userId;

            int i = VaccinationEvent.Update(o);
            if (i > 0)
            {
                if (done || o.NonVaccinationReason.KeepChildDue == false)
                    RescheduleNextDose(o,false);
                
                if (o.VaccineLotId > 0)
                {
                    StockManagementLogic sml = new StockManagementLogic();
                    ItemTransaction it = sml.Vaccinate(o.HealthFacility, o);
                }
            }
            return o;
        }

        /// <summary>
        /// Updates Vaccination Event with default data
        /// </summary>
        /// <param name="ve">Vaccination Event to be updated</param>
        /// <returns></returns>
        public VaccinationEvent RemoveVaccinationEvent(VaccinationEvent ve, int userId)
        {
            if (ve == null)
                throw new ArgumentNullException("ve");
            if (userId <= 0)
                throw new ArgumentException("userId");

            int lotid = ve.VaccineLotId;
            HealthFacility facility = ve.HealthFacility;

            ve.VaccineLotId = 0;
            ve.VaccineLotText = String.Empty;
            ve.VaccinationDate = ve.ScheduledDate;
            ve.Notes = String.Empty;
            ve.VaccinationStatus = false;
            ve.NonvaccinationReasonId = 0;
            ve.HealthFacilityId = ve.Appointment.ScheduledFacilityId;

            ve.ModifiedOn = DateTime.Now;
            ve.ModifiedBy = userId;
            int i = VaccinationEvent.Update(ve);
            if (i > 0)
            {
                RescheduleNextDose(ve, true);
                //update balance
                StockManagementLogic sml = new StockManagementLogic();
                int update = sml.ClearBalance(facility,lotid);
            }
            return ve;
        }
    }
}
