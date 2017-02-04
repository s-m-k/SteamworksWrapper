CC=gcc
CXX=g++
RM=rm -f
LIB=NativeSource/lib
SRC=NativeSource/src
INC=NativeSource/inc
MARCH = -m$(ARCH)
CPPFLAGS=-g $(MARCH) -I/usr/include/root -fPIC -std=c++11 -I$(INC)
LDFLAGS=-g $(MARCH)
LDLIBS=$(LIBS)
OUTDIR=Assets/Plugins/SteamworksWrapper/bin/$(UNITYARCH)

CPPS=Friends.cpp Leaderboard.cpp SteamworksWrapper.cpp User.cpp UserStats.cpp
SRCS=$(patsubst %,$(SRC)/%,$(CPPS))
OBJS=$(subst .cpp,.o,$(SRCS))

all: steamworks

steamworks: $(OBJS)
	mkdir -p $(OUTDIR)
	$(CXX) $(LDFLAGS) -Wall -Wl,-z,origin '-Wl,-rpath,$$ORIGIN' $(SHARED) -o $(OUTDIR)/$(TARGETNAME) $(OBJS) $(LDLIBS) 

depend: .depend

.depend: $(SRCS)
	$(RM) ./.depend
	$(CXX) $(CPPFLAGS) -MM $^>>./.depend;

clean:
	$(RM) $(OBJS)

distclean: clean
	$(RM) *~ .depend

include .depend
