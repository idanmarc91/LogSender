namespace LogSender.Utilities
{
    public static class Constant
    {
        public enum ParamListIndexs
        {
            OS,
            REPORTING_COMPUTER,
            CLIENT_TIME ,
            FULL_SERVER_TIME ,
            PROCESS_ID ,
            PROCESS_NAME ,
            PROCESS_PATH ,
            PROTOCOL ,
            STATUS ,
            SOURCE_PORT ,
            DESTINATION_PORT ,
            DIRECTION ,
            CAST_TYPE,
            SCRAMBLE_STATE ,
            SOURCE_IP ,
            DESTINATION_IP ,
            SEQUANCE_NUMBER ,
            SUB_SEQUANCE_NUMBER ,
            USER_NAME ,
            MOG_COUNTER ,
            DESTINATION_PATH ,
            REASON ,
            DLL_PATH ,
            DLL_NAME ,
            //PARENT_PATH ,
            //PARENT_NAME ,
            CHAIN_ARRAY
        }

        public static readonly string OPERATING_SYSTEM = SystemFunctions.GetOperatingSystem();

        public const int MOG_ROW_SIZE = 2120;
        public const int CYB_ROW_SIZE = 1600;
        public const int DLL_ROW_SIZE = 2064;
        public const int FSA_ROW_SIZE = 2584;

        public static readonly string DOMAIN_FULL_NAME = SystemFunctions.GetFullDomainName();
        public static readonly string DOMAIN_NAME = SystemFunctions.ExtractDomainName() ;
        public static readonly string DOMAIN_IP = SystemFunctions.GetDomainIp() ;
    }
}
