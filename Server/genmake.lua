project "Server"
    category "Server"
    kind "ConsoleApp"
    language "C#"

    linksystemlibs {
        "System",
        "System.Core",
    }

    linkprojects {
        "Manos",
        "Apache.Cassandra",
    }
        
    linkfiles {
        "../Binaries/Thrift",
    }

    compilefiles {
        "main.cs",
        "cassandra.cs",
    }

done "Server"
