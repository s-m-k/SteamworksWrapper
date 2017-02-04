ARCH=32
UNITYARCH=x86
LIBS=-L$(LIB)/linux32 -lsteam_api
TARGETNAME=SteamworksWrapper.so
SHARED=-shared
include common.mk
