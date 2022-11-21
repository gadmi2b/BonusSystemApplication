using BonusSystemApplication.Models.ViewModels.FormViewModel;
using BonusSystemApplication.UserIdentiry;
using Microsoft.Data.SqlClient.Server;
using System.Reflection;
using System.Security.Cryptography;

namespace BonusSystemApplication.Models.BusinessLogic
{
    public static class FormDataHandler
    {
        public static bool IsObjectivesSignaturePossible(Form form)
        {
            if (form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }
        public static bool IsResultsSignaturePossible(Form form)
        {
            if (form.IsObjectivesFreezed && form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }

        public static void PutUserSignature(ref Dictionary<string, object> propertiesValues)
        {
            if (propertiesValues == null)
            {
                throw new ArgumentNullException(nameof(propertiesValues));
            }

            #region Description of logic
            /*
             * LOGIC: find the first bool and and get its value inside of all values
             *        there are could be max two bools with same values (isSignedId and isRejectedId)
             *        and one string value (signatureId)
             */
            #endregion

            bool isSigned = false;
            foreach (var value in propertiesValues.Values)
            {
                if (value != null &&
                   value.GetType() == typeof(bool))
                {
                    isSigned = (bool)value;
                    break;
                }
            }

            string userSignature = string.Empty;
            if (isSigned)
            {
                userSignature = UserData.GetUserSignature();
            }

            #region Description of logic
            /*
             * LOGIC: find the first string value inside all values
             *        and assign a User signature to it
             */
            #endregion

            foreach (string key in propertiesValues.Keys)
            {
                if (propertiesValues.TryGetValue(key, out var value))
                {
                    if (value.GetType() == typeof(string))
                    {
                        propertiesValues[key] = userSignature;
                        break;
                    }
                }
            }
        }

        public static void UpdateSignatureFormData(Form form, Dictionary<string, object> propertiesValues)
        {
            foreach (string property in propertiesValues.Keys)
            {
                var value = propertiesValues[property];
                PropertyInfo propertyInfo = form.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(form, value);
                }
            }
        }
        public static void UpdateLastSavedFormData(Form form)
        {
            form.LastSavedBy = UserData.GetUserName();
            form.LastSavedDateTime = DateTime.Now;
        }



        public static void UpdateObjectivesResultsFormData(Form form,
                                                           ObjectivesDefinition objectivesDefinition,
                                                           ResultsDefinition resultsDefinition)
        {
            if (IsObjectivesResultsSavePossible(form))
            {
                // Objectives updating
                if (Enum.TryParse(objectivesDefinition.Period, out Periods period))
                {
                    form.Period = period;
                }
                else
                {
                    throw new ArgumentException("Unknown value of period. Form save is impossible");
                }

                if (Int32.TryParse(objectivesDefinition.Year, out int year))
                {
                    form.Year = year;
                }
                else
                {
                    throw new ArgumentException("Unknown value of year. Form save is impossible");
                }

                form.EmployeeId = objectivesDefinition.EmployeeId;
                form.ManagerId = objectivesDefinition.ManagerId;
                form.ApproverId = objectivesDefinition.ApproverId;
                form.WorkprojectId = objectivesDefinition.WorkprojectId;
                form.IsWpmHox = objectivesDefinition.IsWpmHox;

                // TODO: !!! Rows in Objectives and in Results must be the same
                //       !!! incorrect just copy data from obj to objRes
                foreach (Objective obj in objectivesDefinition.Objectives)
                {
                    foreach(ObjectiveResult objRes in form.ObjectivesResults)
                    {
                        objRes.Row = obj.Row;
                        objRes.Statement = obj.Statement;
                        objRes.Description = obj.Description;
                        objRes.IsKey = obj.IsKey;
                        objRes.IsMeasurable = obj.IsMeasurable;
                        objRes.Unit = obj.Unit;
                        objRes.Threshold = obj.Threshold;
                        objRes.Target = obj.Target;
                        objRes.Challenge = obj.Challenge;
                        objRes.WeightFactor = obj.WeightFactor;
                        objRes.KpiUpperLimit = obj.KpiUpperLimit;
                    }
                }

                // Results updating

                form.ManagerComment = resultsDefinition.ManagerComment;
                form.EmployeeComment = resultsDefinition.EmployeeComment;
                form.OtherComment = resultsDefinition.OtherComment;

                // TODO: !!! Rows in Objectives and in Results must be the same
                //       !!! incorrect just copy data from res to objRes
                foreach (Result res in resultsDefinition.Results)
                {
                    foreach (ObjectiveResult objRes in form.ObjectivesResults)
                    {
                        objRes.KeyCheck = res.KeyCheck;
                        objRes.Achieved = res.Achieved;
                        objRes.Kpi = res.Kpi;
                    }
                }
            }
            else if (IsResultsSavePossible(form))
            {
                // Results updating
                form.IsProposalForBonusPayment = resultsDefinition.IsProposalForBonusPayment;

                form.ManagerComment = resultsDefinition.ManagerComment;
                form.EmployeeComment = resultsDefinition.EmployeeComment;
                form.OtherComment = resultsDefinition.OtherComment;

                // TODO: !!! Rows in Objectives and in Results must be the same
                //       !!! incorrect just copy data from res to objRes
                foreach (Result res in resultsDefinition.Results)
                {
                    foreach (ObjectiveResult objRes in form.ObjectivesResults)
                    {
                        objRes.KeyCheck = res.KeyCheck;
                        objRes.Achieved = res.Achieved;
                        objRes.Kpi = res.Kpi;
                    }
                }

                // TODO: calculate OverallKpi = form.OverallKpi
                //       provide values with the same checks as at client side
            }
        }
        private static bool IsObjectivesResultsSavePossible(Form form)
        {
            if(!form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }
        private static bool IsResultsSavePossible(Form form)
        {
            if (form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }
    }
}