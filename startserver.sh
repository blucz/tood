#!/bin/sh

make -j4 Server

if uname | grep CYGWIN > /dev/null; then 
    Server/bin/debug/Server.exe -static=static
else
    mono Server/bin/debug/Server.exe
fi
