namespace ModelsRx
{
    public static class ContextRx
    {
        public static readonly UserRx UserRx;
        
        static ContextRx()
        {
            UserRx = new UserRx();
            
        }
    }
}