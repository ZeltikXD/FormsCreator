using System;

namespace FormsCreator.Core.Shared
{
	public static class Constants
	{
		private static readonly Guid _userRoleId = Guid.Parse("6d0224d8-761f-4e9f-bc37-4b56fe209fcd");
		private static readonly Guid _adminRoleId = Guid.Parse("7428d1fb-0408-4795-b229-67852851cb0b");

        public static Guid UserRoleId => _userRoleId;

		public static Guid AdminRoleId => _adminRoleId;
	}
}