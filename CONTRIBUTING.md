# Contributing to This is MOD(D)ABLE

All commits are tested for compilation automatically. Commits, which do not pass this test should generally be regarded as non-existent.

The MAIN.cs file contains a string variable for versioning info. the useage of this is explained in the following:

The "dev" branch is for the devvelopment of the main game. 
When you add a mayor feature, set the version string to "x.x.(x+1)-dev" and include in your commit message **ver: x.x.x-dev**
All versions in the dev branch should have the "-dev" postfix to their version string.

The "nightly" branch is for quick deployment of new features. 
When one thinks, that a nightly release should be done and the other core devs agree, all features, functions and mechanics introduced since the last deployment have to be thested manually.
If no error occurs, set the version string to "x.(x+1).0-nightly" and include in your commit message **ver: x.x.0-nightly:** \<new features\>
After the nightly branch is updated, the dev branch is updated.
All versions in the nightly branch should have the "-nightly" postfiv to their version string.

The release branch is for deployment of mayor updates.
When all core devs agree, that a major deployment shouuld be made, **all** features, functions and methots have to be tested manually.
If no error occurs, set the version string to "(x+1).0.0" and include in your commit message **\<release date\>: ver: x.0.0** \<new features\> \{eventually no longer supported features\}
After the release branch ist updated, the nightly and the dev branch are updated.
All versions in the releasse branch do not have a postfix to their verrsion string.
