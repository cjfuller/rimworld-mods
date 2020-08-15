#r "paket:
nuget FSharp.Core
nuget Fake.IO.FileSystem
nuget Fake.DotNet.MSBuild
nuget Fake.Core.Target //"

#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators

let targetDir =
    // Environment.environVar "HOME"
    // @@ "Library/Application Support/Steam/steamapps/common/RimWorld/RimWorldMac.app/Mods"
    "C:\\Program Files (x86)\\Steam\\steamapps\\common\\RimWorld\\Mods"

let allModNames =
    [ "CarpetColors"
      "Meatless"
      "PainEmitter"
      "PortableGenerator" ]

let assembliesDir modName = "." @@ modName @@ "Assemblies"
let sourceDir modName = "." @@ modName @@ "Source"

let buildMod modName =
    let projectFile =
        sourceDir modName @@ (modName + ".fsproj")

    if System.IO.File.Exists(projectFile) then
        MSBuild.runRelease id (assembliesDir modName) "Build" [ projectFile ]
        |> ignore


let cleanMod modName = Shell.cleanDir <| assembliesDir modName

let installModLocally modName =
    let targetSubdir = targetDir @@ modName
    printf "Installing %s\n" modName
    Directory.delete targetSubdir
    Shell.copyDir targetSubdir ("." @@ modName) (fun _ -> true)

let removeExtraAssemblies modName =
    let assemblies = !!(assembliesDir modName @@ "*")
    for assembly in assemblies do
        match (System.IO.Path.GetFileName assembly) with
        // TODO(colin): will this cause problems if multiple mods include this?
        // Have a shared library mod that just includes the fsharp stdlib?
        | "FSharp.Core.dll" -> ()
        | x when x = modName + ".dll" -> ()
        | _ -> System.IO.File.Delete assembly

    if System.IO.Directory.Exists(assembliesDir modName) then
        for subdir in System.IO.Directory.GetDirectories(assembliesDir modName) do
            printf "%s\n" subdir
            System.IO.Directory.Delete(subdir, true)


Target.create "clean" (fun _ -> List.map cleanMod allModNames |> ignore)

Target.create "build" (fun _ -> List.map buildMod allModNames |> ignore)

Target.create "remove-extra-assemblies" (fun _ ->
    List.map removeExtraAssemblies allModNames
    |> ignore)


Target.create "install" (fun _ -> List.map installModLocally allModNames |> ignore)

open Fake.Core.TargetOperators

"clean"
==> "build"
==> "remove-extra-assemblies"
==> "install"

Target.runOrList ()
