ARCH=32
UNITYARCH=x86
LIBS=-L$(LIB)/osx32 -lsteam_api
TARGETNAME=SteamworksWrapperOSX.bundle
SHARED=-dynamiclib
include common.mk
