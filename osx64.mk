ARCH=64
UNITYARCH=x86_64
LIBS=-L$(LIB)/osx32 -lsteam_api
TARGETNAME=SteamworksWrapperOSX64.so
SHARED=-dynamiclib
include common.mk
