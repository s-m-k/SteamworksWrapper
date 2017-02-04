CC=gcc
CXX=g++
RM=rm -f
LIB=NativeSource/lib
SRC=NativeSource/src
INC=NativeSource/inc
MARCH = -m$(ARCHSUFFIX)
CPPFLAGS=-g $(MARCH) -I/usr/include/root -fPIC -std=c++11 -I$(INC)
LDFLAGS=-g $(MARCH)
LDLIBS=$(LIBS) -L$(LIB)/linux$(ARCHSUFFIX) -lsteam_api$(ARCHLIBSUFFIX)
OUTDIR=NativeSource/build

SRCS=$(SRC)/Friends.cpp $(SRC)/Leaderboard.cpp $(SRC)/SteamworksWrapper.cpp $(SRC)/User.cpp $(SRC)/UserStats.cpp
OBJS=$(subst .cpp,.o,$(SRCS))

all: steamworks

steamworks: $(OBJS)
	$(CXX) $(LDFLAGS) $(MARCH) -Wall -shared -o $(OUTDIR)/SteamworksWrapper$(ARCHLIBSUFFIX).so $(OBJS) $(LDLIBS) 

depend: .depend

.depend: $(SRCS)
	$(RM) ./.depend
	$(CXX) $(CPPFLAGS) -MM $^>>./.depend;

clean:
	$(RM) $(OBJS)

distclean: clean
	$(RM) *~ .depend

include .depend
