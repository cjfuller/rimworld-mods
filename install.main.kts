// Installs the mods locally into the rimworld app.
// Before running this, be sure to use visual studio to build the PainEmitter assembly.

import java.io.File

val targetDir = "/Users/colin/Library/Application Support/Steam/steamapps/common/RimWorld/RimWorldMac.app/Mods"

fun assembliesDir(modName: String): String =
    "./$modName/Assemblies"

fun removeExtraAssemblies(modName: String) {
    val dir = assembliesDir(modName)
    val assemblies = File(dir).list()
    assemblies.forEach { a -> 
        when {
            a == "FSharp.Core.dll" -> Unit
            a == "${modName}.dll" -> Unit
            else -> File("$dir/$a").deleteRecursively()
        }
    }
}

fun installModLocally(modName: String) {
    val targetSubdir = "$targetDir/$modName"
    println("Installing $modName")
    File(targetSubdir).deleteRecursively()
    File("./$modName").copyRecursively(File(targetSubdir))
}

val allModNames = listOf(
    "CarpetColors",
    "Meatless",
    "PainEmitter",
    "PortableGenerator"
)

allModNames.forEach { 
    removeExtraAssemblies(it)
    installModLocally(it)
}