using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DataManagement.db {
    public class UserSession {
        private readonly ProtectedSessionStorage _storage;
        private const string StorageKey = "ud_user_session";

        public UserSession(ProtectedSessionStorage storage) {
            _storage = storage;
        }

        public int? UserId { get; private set; }
        public string Login { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Role { get; private set; } = string.Empty;

        public bool IsSessionActive { get; private set; }
        public bool IsAuthenticated => IsSessionActive && !IsGuest && UserId > 0;
        public bool IsGuest { get; private set; }
        public bool IsLoaded { get; private set; }

        public event Action? OnChange;
        public async Task LoadSessionAsync() {
            if (IsLoaded) return;
            try {
                var result = await _storage.GetAsync<SessionData>(StorageKey);
                if (result.Success && result.Value != null) {
                    var data = result.Value;
                    UserId = data.UserId;
                    Login = data.Login;
                    Name = data.Name;
                    Role = data.Role;
                    IsGuest = data.IsGuest;
                    IsSessionActive = data.IsSessionActive;
                }
            }
            catch {
                // Игнорируем ошибку при пререндеринге
            }
            IsLoaded = true;
            NotifyStateChanged();
        }

        public async Task StartUserSessionAsync(User user) {
            UserId = user.id;
            Login = user.login ?? "";
            Name = user.name ?? user.login ?? "Сотрудник";
            Role = user.roly ?? "Сотрудник";
            IsGuest = false;
            IsSessionActive = true;
            IsLoaded = true;

            await SaveToStorageAsync();
            NotifyStateChanged();
        }
        public async Task StartGuestSessionAsync(string guestName = "Гость") {
            UserId = null;
            Login = "guest";
            Name = guestName;
            Role = "Гость";
            IsGuest = true;
            IsSessionActive = true;
            IsLoaded = true;

            await SaveToStorageAsync();
            NotifyStateChanged();
        }

        public async Task LogoutAsync() {
            UserId = null;
            Login = string.Empty;
            Name = string.Empty;
            Role = string.Empty;
            IsGuest = false;
            IsSessionActive = false;

            await _storage.DeleteAsync(StorageKey);
            NotifyStateChanged();
        }

        private async Task SaveToStorageAsync() {
            var data = new SessionData {
                UserId = UserId,
                Login = Login,
                Name = Name,
                Role = Role,
                IsGuest = IsGuest,
                IsSessionActive = IsSessionActive
            };
            await _storage.SetAsync(StorageKey, data);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        private class SessionData {
            public int? UserId { get; set; }
            public string Login { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public bool IsGuest { get; set; }
            public bool IsSessionActive { get; set; }
        }
    }
}