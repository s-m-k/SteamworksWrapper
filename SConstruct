#!python
import sys
import os.path

availableArchs = ['32', '64']
availableOses = ['linux', 'osx']

AddOption('--os',
	dest='os',
	type='string',
	nargs=1,
	action='store',
	help='Target OS')

AddOption('--arch',
	dest='arch',
	type='string',
	nargs=1,
	action='store',
	help='Target architecture')

os = GetOption('os')
arch = GetOption('arch')

def buildForSystem(os, arch):
	libSuffix = ''
	steamPath = os + arch

	if arch == '64':
		unityArch = 'x86_64'
		libSuffix = '64'
	else:
		unityArch = 'x86'

	if arch == '64' and os == 'osx':
		steamPath = 'osx32'
	
	env = Environment()

	libSources = Glob('NativeSource/src/*.cpp')
	targetPath = 'Assets/Plugins/SteamworksWrapper/bin/' + unityArch + '/'
	libTarget = 'SteamworksWrapper' + libSuffix

	env.Append(CCFLAGS='-g -I/usr/include/root -INativeSource/inc -m' + arch + ' -fPIC -std=c++11 -Wall')
	env.Append(LIBPATH=['NativeSource/lib/' + steamPath])
	env.Append(LIBS=['steam_api'])

	if os == 'osx':
		env.Append(LINKFLAGS=['-Wl,-rpath,\'$$ORIGIN\'','-m' + arch])
		targetPath += libTarget + '.bundle/Contents/MacOS/'
	else:
		env.Append(LINKFLAGS=['-Wl,-z,origin','-Wl,-rpath,\'$$ORIGIN\'','-m' + arch])

	env.LoadableModule(target = targetPath + libTarget, source = libSources)


if not os in availableOses:
	print('Expected OS (--os): ' + ', '.join(availableOses))
	exit()

if not arch in availableArchs:
	print('Expected architecture (--arch): ' + ', '.join(availableArchs))
	exit()

buildForSystem(os, arch)
