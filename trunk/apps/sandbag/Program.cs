namespace munimji.apps.sandbag {
    internal class Program {
        private static void Main(string[] args) {
            EntityFramework.Initialize();
            EntityFramework.SetupForFirstTimeUse();
        }
    }
}