import System.Environment 
#import GitSharp from """C:\home\dev\oss\git-dot-aspx\lib\GitSharp\GitSharp.dll"""
import GitSharp 
target default:
	repo = Repository(System.IO.Directory.GetCurrentDirectory())
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