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
        "../manos/libs/Nini",
        "../Binaries/C5"
    }

    if platform.is("windows") then
        linkfiles "../Binaries/Mono.Posix"
    else
        linksystemlibs "Mono.Posix"
        copyprojects {
            "libev",
            "libmanos",
            "libeio",
        }
    end

    compilefiles {
        "../manos/src/Manos/Manos.IO.Libev/IOLoop.cs",
        "../manos/src/Manos/Manos.IO/IOStream.cs",
        "../manos/src/Manos/Manos.IO/SendFileOperation.cs",
        "../manos/src/Manos/Manos.Managed/AsyncWatcher.cs",
        "../manos/src/Manos/Manos.Managed/IOLoop.cs",
        "../manos/src/Manos/Manos.Managed/Libeio.cs",
        "../manos/src/Manos/Manos.Managed/SendFileOperation.cs",
        "../manos/src/Manos/Manos.Managed/Socket.cs",
        "../manos/src/Manos/Manos/Loop.cs",
        "../manos/src/Manos/Manos/BaseWatcher.cs",
        "../manos/src/Manos/Manos/DeleteAttribute.cs",
        "../manos/src/Manos/Manos/GetAttribute.cs",
        "../manos/src/Manos/Manos/HeadAttribute.cs",
        "../manos/src/Manos/Manos/HttpMethodAttribute.cs",
        "../manos/src/Manos/Manos/IgnoreAttribute.cs",
        "../manos/src/Manos/Manos/ManosApp.cs",
        "../manos/src/Manos/Manos/ManosContext.cs",
        "../manos/src/Manos/Manos/IManosModule.cs",
        "../manos/src/Manos/Manos/ManosModule.cs",
        "../manos/src/Manos/Manos/ManosMimeTypes.cs",
        "../manos/src/Manos/Manos/OptionsAttribute.cs",
        "../manos/src/Manos/Manos/PostAttribute.cs",
        "../manos/src/Manos/Manos/PutAttribute.cs",
        "../manos/src/Manos/Manos/TraceAttribute.cs",
        "../manos/src/Manos/Manos.Http/HState.cs",
        "../manos/src/Manos/Manos.Http/HttpCallback.cs",
        "../manos/src/Manos/Manos.Http/HttpCookie.cs",
        "../manos/src/Manos/Manos.Http/HttpDataCallback.cs",
        "../manos/src/Manos/Manos.Http/HttpErrorCallback.cs",
        "../manos/src/Manos/Manos.Http/HttpFormDataHandler.cs",
        "../manos/src/Manos/Manos.Http/HttpMethod.cs",
        "../manos/src/Manos/Manos.Http/HttpMultiPartFormDataHandler.cs",
        "../manos/src/Manos/Manos.Http/HttpParser.cs",
        "../manos/src/Manos/Manos.Http/HttpStream.cs",
        "../manos/src/Manos/Manos.Http/HttpStreamWriterWrapper.cs",
        "../manos/src/Manos/Manos.Http/IHttpBodyHandler.cs",
        "../manos/src/Manos/Manos.Http/ParserSettings.cs",
        "../manos/src/Manos/Manos.Http/ParserType.cs",
        "../manos/src/Manos/Manos.Http/State.cs",
        "../manos/src/Manos/Manos.Http/HttpEntity.cs",
        "../manos/src/Manos/Manos.Http/HttpException.cs",
        "../manos/src/Manos/Manos.Http/HttpHeaders.cs",
        "../manos/src/Manos/Manos.Http/HttpRequest.cs",
        "../manos/src/Manos/Manos.Http/HttpResponse.cs",
        "../manos/src/Manos/Manos.Http/HttpServer.cs",
        "../manos/src/Manos/Manos.Http/HttpTransaction.cs",
        "../manos/src/Manos/Manos.Http/HttpUtility.cs",
        "../manos/src/Manos/Manos.Http/IHttpRequest.cs",
        "../manos/src/Manos/Manos.Http/IHttpResponse.cs",
        "../manos/src/Manos/Manos.Http/IHttpTransaction.cs",
        "../manos/src/Manos/Manos.Http/UploadedFile.cs",
        "../manos/src/Manos/Manos.Routing/RouteHandler.cs",
        "../manos/src/Manos/Manos.IO/ConnectionAcceptedEventArgs.cs",
        "../manos/src/Manos/Manos.IO/IOLoop.cs",
        "../manos/src/Manos/Manos.IO.Libev/IOStream.cs",
        "../manos/src/Manos/Manos.IO/NopWriteOperation.cs",
        "../manos/src/Manos/Manos.IO/SendBytesOperation.cs",
        "../manos/src/Manos/Manos.IO.Libev/SocketStream.cs",
        "../manos/src/Manos/Manos.IO/IWriteOperation.cs",
        "../manos/src/Manos/Manos/IManosContext.cs",
        "../manos/src/Manos/Manos.Logging/IManosLogger.cs",
        "../manos/src/Manos/Manos.Logging/ManosConsoleLogger.cs",
        "../manos/src/Manos/Manos.Logging/LogLevel.cs",
        "../manos/src/Manos/Manos.Testing/MockManosModule.cs",
        "../manos/src/Manos/Manos.Http.Testing/MockHttpTransaction.cs",
        "../manos/src/Manos/Manos.Http.Testing/MockHttpRequest.cs",
        "../manos/src/Manos/Manos.Testing/ManosContextStub.cs",
        "../manos/src/Manos/Manos.Testing/ManosAppStub.cs",
        "../manos/src/Manos/Manos.Testing/ManosBrowser.cs",
        "../manos/src/Manos/Manos.Http.Testing/MockHttpResponse.cs",
        "../manos/src/Manos/Manos.Routing.Testing/MockManosTarget.cs",
        "../manos/src/Manos/Manos.Routing/IMatchOperation.cs",
        "../manos/src/Manos/Manos.Routing/RegexMatchOperation.cs",
        "../manos/src/Manos/Manos.Routing/MatchOperationFactory.cs",
        "../manos/src/Manos/Manos.Routing/StringMatchOperation.cs",
        "../manos/src/Manos/Manos.Routing/NopMatchOperation.cs",
        "../manos/src/Manos/Manos.Routing/MatchType.cs",
        "../manos/src/Manos/Manos/HttpMethods.cs",
        "../manos/src/Manos/Assembly/AssemblyInfo.cs",
        "../manos/src/Manos/Manos/IRepeatBehavior.cs",
        "../manos/src/Manos/Manos/IterativeRepeatBehavior.cs",
        "../manos/src/Manos/Manos/InfiniteRepeatBehavior.cs",
        "../manos/src/Manos/Manos/RepeatBehavior.cs",
        "../manos/src/Manos/Manos/TimeoutCallback.cs",
        "../manos/src/Manos/Manos/Timeout.cs",
        "../manos/src/Manos/Manos/AppHost.cs",
        "../manos/src/Manos/Manos.Caching/CacheItemCallback.cs",
        "../manos/src/Manos/Manos.Caching/CacheOpCallback.cs",
        "../manos/src/Manos/Manos.Caching/IManosCache.cs",
        "../manos/src/Manos/Manos.Caching/ManosInProcCache.cs",
        "../manos/src/Manos/Manos.Routing/ActionTarget.cs",
        "../manos/src/Manos/Manos.Routing/IManosTarget.cs",
        "../manos/src/Manos/Manos.Routing/ParameterizedAction.cs",
        "../manos/src/Manos/Manos.Routing/ParameterizedActionFactory.cs",
        "../manos/src/Manos/Manos.Routing/ParameterizedActionTarget.cs",
        "../manos/src/Manos/Manos.Routing/ManosAction.cs",
        "../manos/src/Manos/Manos.Routing/SimpleMatchOperation.cs",
        "../manos/src/Manos/Manos/IManosPipe.cs",
        "../manos/src/Manos/Manos/ManosPipe.cs",
        "../manos/src/Manos/Manos/Pipeline.cs",
        "../manos/src/Manos/Manos.Collections/DataDictionary.cs",
        "../manos/src/Manos/Manos.Collections/ByteBuffer.cs",
        "../manos/src/Manos/Manos/RouteAttribute.cs",
        "../manos/src/Manos/Manos/UnsafeString.cs",
        "../manos/src/Manos/Manos.Routing/HtmlFormDataTypeConverter.cs",
        "../manos/src/Manos/Manos.Template/TemplateEnvironment.cs",
        "../manos/src/Manos/Manos.Template/TemplateLibrary.cs",
        "../manos/src/Manos/Manos.Template/TemplateParser.cs",
        "../manos/src/Manos/Manos.Template/TemplateTokenizer.cs",
        "../manos/src/Manos/Manos.Template/TemplateEngine.cs",
        "../manos/src/Manos/Manos.Template/ManosTemplate.cs",
        "../manos/src/Manos/Manos.Template/IManosTemplate.cs",
        "../manos/src/Manos/Manos.Template/ITemplateCodegen.cs",
        "../manos/src/Manos/Manos.Template/Expression.cs",
        "../manos/src/Manos/Manos.Template/TemplateFactory.cs",
        "../manos/src/Manos/Libeio/Libeio.cs",
        "../manos/src/Manos/Libev/AsyncWatcher.cs",
        "../manos/src/Manos/Libev/CheckWatcher.cs",
        "../manos/src/Manos/Libev/EventTypes.cs",
        "../manos/src/Manos/Libev/IdleWatcher.cs",
        "../manos/src/Manos/Libev/IOWatcher.cs",
        "../manos/src/Manos/Libev/Loop.cs",
        "../manos/src/Manos/Libev/LoopType.cs",
        "../manos/src/Manos/Libev/PrepareWatcher.cs",
        "../manos/src/Manos/Libev/TimerWatcher.cs",
        "../manos/src/Manos/Libev/UnloopType.cs",
        "../manos/src/Manos/Libev/UnmanagedWatcherCallback.cs",
        "../manos/src/Manos/Libev/Watcher.cs",
        "../manos/src/Manos/Manos.Http/HttpBufferedBodyHandler.cs",
        "../manos/src/Manos/Manos/IManosRun.cs",
        "../manos/src/Manos/Manos.IO.Libev/UdpReceiver.cs",
        "../manos/src/Manos/Manos/ManosConfig.cs",
        "../manos/src/Manos/Manos.IO/PosixSendFileOperation.cs",
        "../manos/src/Manos/Manos.IO/CopyingSendFileOperation.cs",
        "../manos/src/Manos/Manos.IO.Libev/SecureSocketStream.cs",
        "../manos/src/Manos/Manos.IO.Libev/PlainSocketStream.cs",
        "../manos/src/Manos/Manos.Threading/Boundary.cs",
        "../manos/src/Manos/Manos.Threading/BoundaryExtensions.cs",
        "../manos/src/Manos/Manos.Threading/IBoundary.cs",
    }

done "Manos"

if (not platform.is("windows")) then 
    project "libev"
        category "_Libraries"
        kind "SharedLib"
        language "C"
        flags { "Unsafe" }

        includedirs "../manos/src/libev"

        compilefiles {
            "../manos/src/libev/ev.c",
            "../manos/src/libev/ev_win32.c",
            "../manos/src/libev/event.c",
        }

    done "libev"

    project "libeio"
        category "_Libraries"
        kind "SharedLib"
        language "C"
        flags { "Unsafe" }

        includedirs "../manos/src/libeio"

        compilefiles {
            "../manos/src/libeio/eio.c",
        }

    done "libeio"

    project "libmanos"
        category "_Libraries"
        kind "SharedLib"
        language "C"
        flags { "Unsafe" }

        includedirs "../manos/src/libmanos"

        compilefiles {
            "../manos/src/libmanos/manos.c",
            "../manos/src/libmanos/manos_libev.c",
            "../manos/src/libmanos/manos_tls.c",
            "../manos/src/libmanos/manos_socket.c",
        }

    done "libmanos"
end
