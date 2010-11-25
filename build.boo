import System.Environment 
import System.IO

import GitSharp 

repo as Repository
version as string
out = ".\\out"
slnFile = ".\\src\\ntemplate.sln"
configuration = 'Release'

def getRepo:
	if repo == null:
		repo = Repository(Directory.GetCurrentDirectory())
	return repo 
	
def setVersion:
	majorVersion = File.ReadAllText('version').Trim()
	buildNumber = env('BUILD_NUMBER')
	buildNumber = "0" if (string.IsNullOrEmpty(buildNumber))
	version = majorVersion + "." + buildNumber
	print 'version: ' + version

def reportVersionToTeamCity:
	print "##teamcity[buildNumber '${version}']"

def writeBuildInfo:
	sourceOutFilename = Path.Combine(out, 'BuildInfo.txt')
	lines = ["Build info:","===================="]
	lines.Add(" at: " + System.DateTime.UtcNow)
	lines.Add(" branch: " + repo.CurrentBranch.Name)
	lines.Add(" commit: " + repo.Head.CurrentCommit.Hash)
	lines.Add(" source: " + env("BUILD_VCS_URL"))
	
	File.WriteAllLines(sourceOutFilename, array(string, lines))

def revertAssemblyInfo:
	index = Index(repo)
	fullpath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "src/SolutionAssemblyInfo.cs"))
	index.Checkout(fullpath)

setVersion()
reportVersionToTeamCity()
getRepo()


desc "set build configuration to Debug"
target debug:
	configuration = 'Debug'


desc "initialization"
target init, (generate_assembly_info):
	rmdir(out)
	mkdir(out)
	writeBuildInfo()
	print 'initialized'
	
desc "Generating the shared assemblyInfo with version info"
target generate_assembly_info:
	with generate_assembly_info():
		.file = 'src\\SolutionAssemblyInfo.cs'
		.version = version
		
	
desc "entry point for the teamcity build"
target default, (init, build, test):
	print 'default target completed'
	

desc "Build the sources"
target build:
	msbuild(file: slnFile, configuration: configuration)

desc "run unit tests"
target test:
	test_assemblies = (".\\src\\NTemplate.Tests\\bin\\${configuration}\\NTemplate.Tests.dll",)
	nunit(assemblies: test_assemblies, toolPath: ".\\src\\packages\\NUnit.2.5.7.10213\\Tools\\nunit-console.exe" )