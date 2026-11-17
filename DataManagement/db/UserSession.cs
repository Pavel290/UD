namespace DataManagement.db {
    public class UserSession {
        public int UserId { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsAuthenticated => UserId > 0;
    }
}
