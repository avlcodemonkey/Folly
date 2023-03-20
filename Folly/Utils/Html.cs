namespace Folly
{
    public enum DataToggle
    {
        Autocomplete,
        CollapsibleList,
        ContextHelp,
        Dialog,
        DoTable,
        Dropdown,
        Hide,
        NavMenu,
        Tab
    }

    public enum Button
    {
        Primary,
        Secondary,
        Dark,
        Error,
        Success,
        Icon,
        IconOnly
    }

    public static class IconChar
    {
        public const string Dismiss = "&#10006;";
        public const string Edit = "&#9998;";
    }

    public enum AlertType
    {
        Success,
        Error
    }

    public enum Icon
    {
        ArrowsCw,
        BellAlt,
        Bug,
        Calendar,
        Cancel,
        ChartBar,
        Clone,
        Corner,
        Database,
        Down,
        Edit,
        Eyedropper,
        EmoUnhappy,
        Fire,
        Heartbeat,
        Help,
        Home,
        Info,
        Key,
        Lamp,
        ListAlt,
        ListBullet,
        ListNumbered,
        Login,
        Logout,
        Max,
        Menu,
        Min,
        Minus,
        Move,
        Pencil,
        Plus,
        Search,
        Sort,
        SortDown,
        SortUp,
        Th,
        ToEnd,
        ToEndAlt,
        ToStart,
        ToStartAlt,
        Trash,
        Unlock,
        Up,
        User,
        Users
    }

    public enum HttpVerb
    {
        Get,
        Put,
        Post,
        Delete
    }
}
