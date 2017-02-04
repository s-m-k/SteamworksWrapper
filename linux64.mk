ARCH=64
UNITYARCH=x86_64
LIBS=-L$(LIB)/linux64 -lsteam_api64
TARGETNAME=libSteamworksWrapper64.so
SHARED=-shared
include common.mk
