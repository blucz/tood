project "Manos"
    category "_Libraries"
    kind "SharedLib"
    language "C#"
    flags { "Unsafe" }

    linksystemlibs {
        "System",
        "System.Core",
        "System.Xml",
    }

    linkfiles {
        "upstream/libs/Nini",
        "../Binaries/C5"
    }

    if platform.is("windows") then
        linkfiles "../Binaries/Mono.Posix"
    else
        linksystemlibs "Mono.Posix"
        copyprojects {
            "ev",
            "manos",
            "eio",
        }
    end

    compilefiles {
        "upstream/src/Manos/Manos.IO.Libev/IOLoop.cs",
        "upstream/src/Manos/Manos.IO/IOStream.cs",
        "upstream/src/Manos/Manos.IO/SendFileOperation.cs",
        "upstream/src/Manos/Manos.Managed/AsyncWatcher.cs",
        "upstream/src/Manos/Manos.Managed/IOLoop.cs",
        "upstream/src/Manos/Manos.Managed/Libeio.cs",
        "upstream/src/Manos/Manos.Managed/SendFileOperation.cs",
        "upstream/src/Manos/Manos.Managed/Socket.cs",
        "upstream/src/Manos/Manos/Loop.cs",
        "upstream/src/Manos/Manos/BaseWatcher.cs",
        "upstream/src/Manos/Manos/DeleteAttribute.cs",
        "upstream/src/Manos/Manos/GetAttribute.cs",
        "upstream/src/Manos/Manos/HeadAttribute.cs",
        "upstream/src/Manos/Manos/HttpMethodAttribute.cs",
        "upstream/src/Manos/Manos/IgnoreAttribute.cs",
        "upstream/src/Manos/Manos/ManosApp.cs",
        "upstream/src/Manos/Manos/ManosContext.cs",
        "upstream/src/Manos/Manos/IManosModule.cs",
        "upstream/src/Manos/Manos/ManosModule.cs",
        "upstream/src/Manos/Manos/ManosMimeTypes.cs",
        "upstream/src/Manos/Manos/OptionsAttribute.cs",
        "upstream/src/Manos/Manos/PostAttribute.cs",
        "upstream/src/Manos/Manos/PutAttribute.cs",
        "upstream/src/Manos/Manos/TraceAttribute.cs",
        "upstream/src/Manos/Manos.Http/HState.cs",
        "upstream/src/Manos/Manos.Http/HttpCallback.cs",
        "upstream/src/Manos/Manos.Http/HttpCookie.cs",
        "upstream/src/Manos/Manos.Http/HttpDataCallback.cs",
        "upstream/src/Manos/Manos.Http/HttpErrorCallback.cs",
        "upstream/src/Manos/Manos.Http/HttpFormDataHandler.cs",
        "upstream/src/Manos/Manos.Http/HttpMethod.cs",
        "upstream/src/Manos/Manos.Http/HttpMultiPartFormDataHandler.cs",
        "upstream/src/Manos/Manos.Http/HttpParser.cs",
        "upstream/src/Manos/Manos.Http/HttpStream.cs",
        "upstream/src/Manos/Manos.Http/HttpStreamWriterWrapper.cs",
        "upstream/src/Manos/Manos.Http/IHttpBodyHandler.cs",
        "upstream/src/Manos/Manos.Http/ParserSettings.cs",
        "upstream/src/Manos/Manos.Http/ParserType.cs",
        "upstream/src/Manos/Manos.Http/State.cs",
        "upstream/src/Manos/Manos.Http/HttpEntity.cs",
        "upstream/src/Manos/Manos.Http/HttpException.cs",
        "upstream/src/Manos/Manos.Http/HttpHeaders.cs",
        "upstream/src/Manos/Manos.Http/HttpRequest.cs",
        "upstream/src/Manos/Manos.Http/HttpResponse.cs",
        "upstream/src/Manos/Manos.Http/HttpServer.cs",
        "upstream/src/Manos/Manos.Http/HttpTransaction.cs",
        "upstream/src/Manos/Manos.Http/HttpUtility.cs",
        "upstream/src/Manos/Manos.Http/IHttpRequest.cs",
        "upstream/src/Manos/Manos.Http/IHttpResponse.cs",
        "upstream/src/Manos/Manos.Http/IHttpTransaction.cs",
        "upstream/src/Manos/Manos.Http/UploadedFile.cs",
        "upstream/src/Manos/Manos.Routing/RouteHandler.cs",
        "upstream/src/Manos/Manos.IO/ConnectionAcceptedEventArgs.cs",
        "upstream/src/Manos/Manos.IO/IOLoop.cs",
        "upstream/src/Manos/Manos.IO.Libev/IOStream.cs",
        "upstream/src/Manos/Manos.IO/NopWriteOperation.cs",
        "upstream/src/Manos/Manos.IO/SendBytesOperation.cs",
        "upstream/src/Manos/Manos.IO.Libev/SocketStream.cs",
        "upstream/src/Manos/Manos.IO/IWriteOperation.cs",
        "upstream/src/Manos/Manos/IManosContext.cs",
        "upstream/src/Manos/Manos.Logging/IManosLogger.cs",
        "upstream/src/Manos/Manos.Logging/ManosConsoleLogger.cs",
        "upstream/src/Manos/Manos.Logging/LogLevel.cs",
        "upstream/src/Manos/Manos.Testing/MockManosModule.cs",
        "upstream/src/Manos/Manos.Http.Testing/MockHttpTransaction.cs",
        "upstream/src/Manos/Manos.Http.Testing/MockHttpRequest.cs",
        "upstream/src/Manos/Manos.Testing/ManosContextStub.cs",
        "upstream/src/Manos/Manos.Testing/ManosAppStub.cs",
        "upstream/src/Manos/Manos.Testing/ManosBrowser.cs",
        "upstream/src/Manos/Manos.Http.Testing/MockHttpResponse.cs",
        "upstream/src/Manos/Manos.Routing.Testing/MockManosTarget.cs",
        "upstream/src/Manos/Manos.Routing/IMatchOperation.cs",
        "upstream/src/Manos/Manos.Routing/RegexMatchOperation.cs",
        "upstream/src/Manos/Manos.Routing/MatchOperationFactory.cs",
        "upstream/src/Manos/Manos.Routing/StringMatchOperation.cs",
        "upstream/src/Manos/Manos.Routing/NopMatchOperation.cs",
        "upstream/src/Manos/Manos.Routing/MatchType.cs",
        "upstream/src/Manos/Manos/HttpMethods.cs",
        "upstream/src/Manos/Assembly/AssemblyInfo.cs",
        "upstream/src/Manos/Manos/IRepeatBehavior.cs",
        "upstream/src/Manos/Manos/IterativeRepeatBehavior.cs",
        "upstream/src/Manos/Manos/InfiniteRepeatBehavior.cs",
        "upstream/src/Manos/Manos/RepeatBehavior.cs",
        "upstream/src/Manos/Manos/TimeoutCallback.cs",
        "upstream/src/Manos/Manos/Timeout.cs",
        "upstream/src/Manos/Manos/AppHost.cs",
        "upstream/src/Manos/Manos.Caching/CacheItemCallback.cs",
        "upstream/src/Manos/Manos.Caching/CacheOpCallback.cs",
        "upstream/src/Manos/Manos.Caching/IManosCache.cs",
        "upstream/src/Manos/Manos.Caching/ManosInProcCache.cs",
        "upstream/src/Manos/Manos.Routing/ActionTarget.cs",
        "upstream/src/Manos/Manos.Routing/IManosTarget.cs",
        "upstream/src/Manos/Manos.Routing/ParameterizedAction.cs",
        "upstream/src/Manos/Manos.Routing/ParameterizedActionFactory.cs",
        "upstream/src/Manos/Manos.Routing/ParameterizedActionTarget.cs",
        "upstream/src/Manos/Manos.Routing/ManosAction.cs",
        "upstream/src/Manos/Manos.Routing/SimpleMatchOperation.cs",
        "upstream/src/Manos/Manos/IManosPipe.cs",
        "upstream/src/Manos/Manos/ManosPipe.cs",
        "upstream/src/Manos/Manos/Pipeline.cs",
        "upstream/src/Manos/Manos.Collections/DataDictionary.cs",
        "upstream/src/Manos/Manos.Collections/ByteBuffer.cs",
        "upstream/src/Manos/Manos/RouteAttribute.cs",
        "upstream/src/Manos/Manos/UnsafeString.cs",
        "upstream/src/Manos/Manos.Routing/HtmlFormDataTypeConverter.cs",
        "upstream/src/Manos/Manos.Template/TemplateEnvironment.cs",
        "upstream/src/Manos/Manos.Template/TemplateLibrary.cs",
        "upstream/src/Manos/Manos.Template/TemplateParser.cs",
        "upstream/src/Manos/Manos.Template/TemplateTokenizer.cs",
        "upstream/src/Manos/Manos.Template/TemplateEngine.cs",
        "upstream/src/Manos/Manos.Template/ManosTemplate.cs",
        "upstream/src/Manos/Manos.Template/IManosTemplate.cs",
        "upstream/src/Manos/Manos.Template/ITemplateCodegen.cs",
        "upstream/src/Manos/Manos.Template/Expression.cs",
        "upstream/src/Manos/Manos.Template/TemplateFactory.cs",
        "upstream/src/Manos/Libeio/Libeio.cs",
        "upstream/src/Manos/Libev/AsyncWatcher.cs",
        "upstream/src/Manos/Libev/CheckWatcher.cs",
        "upstream/src/Manos/Libev/EventTypes.cs",
        "upstream/src/Manos/Libev/IdleWatcher.cs",
        "upstream/src/Manos/Libev/IOWatcher.cs",
        "upstream/src/Manos/Libev/Loop.cs",
        "upstream/src/Manos/Libev/LoopType.cs",
        "upstream/src/Manos/Libev/PrepareWatcher.cs",
        "upstream/src/Manos/Libev/TimerWatcher.cs",
        "upstream/src/Manos/Libev/UnloopType.cs",
        "upstream/src/Manos/Libev/UnmanagedWatcherCallback.cs",
        "upstream/src/Manos/Libev/Watcher.cs",
        "upstream/src/Manos/Manos.Http/HttpBufferedBodyHandler.cs",
        "upstream/src/Manos/Manos/IManosRun.cs",
        "upstream/src/Manos/Manos.IO.Libev/UdpReceiver.cs",
        "upstream/src/Manos/Manos/ManosConfig.cs",
        "upstream/src/Manos/Manos.IO/PosixSendFileOperation.cs",
        "upstream/src/Manos/Manos.IO/CopyingSendFileOperation.cs",
        "upstream/src/Manos/Manos.IO.Libev/SecureSocketStream.cs",
        "upstream/src/Manos/Manos.IO.Libev/PlainSocketStream.cs",
        "upstream/src/Manos/Manos.Threading/Boundary.cs",
        "upstream/src/Manos/Manos.Threading/BoundaryExtensions.cs",
        "upstream/src/Manos/Manos.Threading/IBoundary.cs",
    }

done "Manos"

if (not platform.is("windows")) then 
    project "ev"
        category "_Libraries"
        kind "SharedLib"
        language "C"
        flags { "Unsafe" }

        includedirs "upstream/src/libev"

        compilefiles {
            "upstream/src/libev/ev.c",
            "upstream/src/libev/ev_win32.c",
            "upstream/src/libev/event.c",
        }

    done "ev"

    project "eio"
        category "_Libraries"
        kind "SharedLib"
        language "C"
        flags { "Unsafe" }

        includedirs "upstream/src/libeio"
        
        defines "_GNU_SOURCE"

        compilefiles {
            "upstream/src/libeio/eio.c",
        }

    done "eio"

    project "manos"
        category "_Libraries"
        kind "SharedLib"
        language "C"
        flags { "Unsafe" }

        linkprojects { "ev", "eio" }

        includedirs {
            "upstream/src/libmanos",
            "upstream/src/libev",
            "upstream/src/libeio",
        }

        compilefiles {
            "upstream/src/libmanos/manos.c",
            "upstream/src/libmanos/manos_libev.c",
            "upstream/src/libmanos/manos_tls.c",
            "upstream/src/libmanos/manos_socket.c",
        }

    done "manos"
end
