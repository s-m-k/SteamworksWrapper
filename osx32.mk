ARCH=32
UNITYARCH=x86
LIBS=-L$(LIB)/osx32 -lsteam_api
TARGETNAME=SteamworksWrapperOSX.so
SHARED=-dynamiclib
include common.mk
