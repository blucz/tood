project "Server"
    category "Server"
    kind "ConsoleApp"
    language "C#"

    linkfiles {
        --"../Binaries/Aquiles",
        --"../Binaries/Thrift",
    }

    linkprojects "Manos"

    compilefiles {
        "main.cs"
    }

done "Server"
