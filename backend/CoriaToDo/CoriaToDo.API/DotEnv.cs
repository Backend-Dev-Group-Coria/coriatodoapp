namespace CoriaToDo.API
{
    public static class DotEnv
    {
        public static void Load()
        {
            // looking for ".env" file in "CoriaToDo" folder
            var dir = Directory.GetParent(Directory.GetCurrentDirectory());
            while (dir != null && dir.Name != "CoriaToDo")
            {
                dir = dir.Parent;
            }
            if (dir == null) return;

            var filePath = Path.Combine(dir.FullName, ".env");
            if (!File.Exists(filePath)) return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split('=',StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

    }
}
