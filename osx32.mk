ARCH=32
LIBS=-L$(LIB)/osx32 -lsteam_api
TARGETNAME=SteamworksWrapperOSX.so
SHARED=-dynamiclib
include common.mk
