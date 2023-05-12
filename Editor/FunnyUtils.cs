using UnityEditor;
using System.IO;

namespace SoFunny.FunnySDK.Editor {

    class FunnyUtils {

        /// <summary>
        /// Copies the path from source to destination and removes any .meta files from the destination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        internal static void Copy(string source, string destination) {
            // Clean up any existing unity plugins or old from previous build...
            var dirPath = Path.GetDirectoryName(destination);
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }

#if UNITY_IOS
            if (Directory.Exists(destination)) {
                Directory.Delete(destination, true);
            }
#else
            if (File.Exists(destination)) {
                FileUtil.DeleteFileOrDirectory(destination);
            }
#endif
            // Copy raw from source...
            FileUtil.CopyFileOrDirectory(source, destination);

#if UNITY_IOS
            // Recursively cleanup meta files...
            RecursiveCleanupMetaFiles(new DirectoryInfo(destination));
#endif
        }

        /// <summary>
        /// Private recursive method to remove .meta files from a directory and all of it's sub directories.
        /// </summary>
        private static void RecursiveCleanupMetaFiles(DirectoryInfo directory) {

            var directories = directory.GetDirectories();
            var files = directory.GetFiles();

            foreach (var file in files) {
                // File is a Unity meta file, clean it up...
                if (file.Extension == ".meta") {
                    FileUtil.DeleteFileOrDirectory(file.FullName);
                }
            }

            // Recurse...
            foreach (var subdirectory in directories) {
                RecursiveCleanupMetaFiles(subdirectory);
            }
        }
    }

}


