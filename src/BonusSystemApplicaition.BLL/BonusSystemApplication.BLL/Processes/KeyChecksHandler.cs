namespace BonusSystemApplication.BLL.Processes
{
    public enum KeyChecks
    {
        KeyCheckNA = 0,                // IsKey = false, kpi = achieved
        KeyCheckKO = 1,                // keycheck KO: kpi is null
        KeyCheckOK = 2,                // keycheck OK: kpi is calculated 
        KeyCheckError = 3,             // unable to calculate kpi (missed Threshold or Target or Challange or Achieved)
        KeyCheckErrorNan = 4,          // Threshold or Target or Challange or Achieved is NaNs
        KeyCheckErrorMonotonic = 5,    // Threshold and Target and Challange - non monotonic sequence
    }
    public class KeyChecksHandler
    {
        private string _keyCheckNA { get; } = "N/A";
        private string _keyCheckKO { get; } = "KO";
        private string _keyCheckOK { get; } = "OK";
        private string _keyCheckError { get; } = "???"; 
        private string _keyCheckErrorNan { get; } = "NaN";
        private string _keyCheckErrorMonotonic { get; } = "Error: Non monotonic";

        public string GetKeyCheck(KeyChecks keyCheck)
        {
            switch (keyCheck) {
                case KeyChecks.KeyCheckNA:
                    return _keyCheckNA;

                case KeyChecks.KeyCheckKO:
                    return _keyCheckKO;

                case KeyChecks.KeyCheckOK:
                    return _keyCheckOK;

                case KeyChecks.KeyCheckError:
                    return _keyCheckError;

                case KeyChecks.KeyCheckErrorNan:
                    return _keyCheckErrorNan;

                case KeyChecks.KeyCheckErrorMonotonic:
                    return _keyCheckErrorMonotonic;

                default:
                    throw new Exception("Undefined KeyCheck");
            }
        }

        public Dictionary<KeyChecks, string> GetKeyChecks()
        {
            Dictionary<KeyChecks, string> keyChecks = new Dictionary<KeyChecks, string>();

            foreach(var item in Enum.GetValues(typeof(KeyChecks)))
            {
                KeyChecks keyCheck = (KeyChecks)item;
                keyChecks.Add(keyCheck, GetKeyCheck(keyCheck));
            }
            return keyChecks;
        }
    }
}