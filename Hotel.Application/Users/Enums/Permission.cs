namespace Hotel.Application.Users.Enums
{
    public enum Permission
    {
        // Role
        RoleCreate,
        RoleEdit,
        RoleDelete,
        RoleView,

        // User
        UserCreate,
        UserEdit,
        UserDelete,
        UserView,

        // Permissions
        PermissionView,

        // Reservation
        ReservationCreate,
        ReservationView,

        // Room
        RoomCreate,
        RoomEdit,
        RoomDelete,
        RoomView,

        // RoomType
        RoomTypeCreate,
        RoomTypeEdit,
        RoomTypeDelete,
        RoomTypeView,

        // RatePlan
        RatePlanCreate,
        RatePlanEdit,
        RatePlanDelete,
        RatePlanView,

        // Guest
        GuestCreate,
        GuestEdit,
        GuestDelete,
        GuestView,

        // FiscalAccount
        FiscalAccountView,

        // TransactionCode
        TransactionCodeCreate,
        TransactionCodeEdit,
        TransactionCodeView,

        // TransactionGroup
        TransactionGroupCreate,
        TransactionGroupEdit,
        TransactionGroupView,

        // Dashboard
        DashboardView
    }
}