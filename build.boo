import System.Environment 
import System.IO

import GitSharp 

desc "initialization"
target init, (generate_assembly_info):
	repo = Repository(Directory.GetCurrentDirectory())
	branch = repo.CurrentBranch
	branch.Reset("HEAD" ResetBehavior.Hard)
	
	print 'initialized'
	
desc "Generating the shared assemblyInfo with version info"
target generate_assembly_info:
	majorVersion = File.ReadAllText('version').Trim()
	buildNumber = env('BUILD_NUMBER')
	buildNumber = "0" if (string.IsNullOrEmpty(buildNumber))
	with generate_assembly_info():
		.file = 'src\\SolutionAssemblyInfo.cs'
		.version = majorVersion + "." + buildNumber
		
	


desc "entry point for the teamcity build"
target default:
	repo = Repository(Directory.GetCurrentDirectory())
	print repo.Head.CurrentCommit.CommitDate
	print repo.Head.CurrentCommit.Hash
	print repo.Head.CurrentCommit.Message
	print repo.Head.CurrentCommit.IsTag
	print repo.CurrentBranch.Name	
	print 'BUILD_NUMBER: ' + env('BUILD_NUMBER')
	print 'build.number: ' + env('build.number')
	print 'momo: ' + env('momo')
	print 'args:' 
	for a in GetCommandLineArgs():
		print '  ' + a