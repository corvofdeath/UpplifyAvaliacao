namespace UpShop.Api.Utils
{
    public class LoggingEvents
    {
        public const int GenerateItems = 1000;
        public const int ListItems = 1001;
        public const int GetItem = 1002;
        public const int InsertItem = 1003;
        public const int UpdateItem = 1004;
        public const int DeleteItem = 1005;

        public const int SerializeJSON = 2000;
        public const int DeserializeJSON = 2001;
        public const int ModelState = 2002;
        public const int MappingOperation = 2003;

        public const int VerifyItemExist = 3000;
        public const int ItemAlreadyExist = 3001;

        public const int GetItemNotFound = 4000;
        public const int UpdateItemNotFound = 4001;

        public const int Unauthorized = 5000;
        public const int UserLogin = 5001;
        public const int PasswordChanged = 5002;
    }
}
