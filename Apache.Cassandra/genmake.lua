project "Apache.Cassandra"
    category "_Libraries"
    kind "SharedLib"
    language "C#"

    linksystemlibs {
        "System",
    }

    linkfiles {
        "../Binaries/Thrift"
    }

    compilefiles {
        "Apache/Cassandra/AuthenticationException.cs",
        "Apache/Cassandra/AuthenticationRequest.cs",
        "Apache/Cassandra/AuthorizationException.cs",
        "Apache/Cassandra/Cassandra.cs",
        "Apache/Cassandra/CfDef.cs",
        "Apache/Cassandra/Column.cs",
        "Apache/Cassandra/ColumnDef.cs",
        "Apache/Cassandra/ColumnOrSuperColumn.cs",
        "Apache/Cassandra/ColumnParent.cs",
        "Apache/Cassandra/ColumnPath.cs",
        "Apache/Cassandra/ConsistencyLevel.cs",
        "Apache/Cassandra/Constants.cs",
        "Apache/Cassandra/Deletion.cs",
        "Apache/Cassandra/IndexClause.cs",
        "Apache/Cassandra/IndexExpression.cs",
        "Apache/Cassandra/IndexOperator.cs",
        "Apache/Cassandra/IndexType.cs",
        "Apache/Cassandra/InvalidRequestException.cs",
        "Apache/Cassandra/KeyCount.cs",
        "Apache/Cassandra/KeyRange.cs",
        "Apache/Cassandra/KeySlice.cs",
        "Apache/Cassandra/KsDef.cs",
        "Apache/Cassandra/Mutation.cs",
        "Apache/Cassandra/NotFoundException.cs",
        "Apache/Cassandra/SlicePredicate.cs",
        "Apache/Cassandra/SliceRange.cs",
        "Apache/Cassandra/SuperColumn.cs",
        "Apache/Cassandra/TimedOutException.cs",
        "Apache/Cassandra/TokenRange.cs",
        "Apache/Cassandra/UnavailableException.cs",
    }

done "Apache.Cassandra"

