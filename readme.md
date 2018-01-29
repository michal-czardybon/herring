Herring
----

Herring is an activity tracker designed for enhancing productivity in computer-related work.

Homepage: http://herring.zohosites.com/

Classes
--------------------------------------------------------------------------------

	ActivitySample
	|   * App : AppInfo
	|   * Title : string
	|   * KeyboardIntensity : double
	|   * MouseIntensity : double
	|
	+-- ActivitySnapshot		// primitive data collected every ~3 seconds
	|       * Time : DateTipe
	|
	+-- ActivityEntry			// composite data being displayed to the user
	        * Share : Double


The Process
--------------------------------------------------------------------------------

Every 3 seconds the applications takes a SNAPSHOT of user activity.

	A snapshot consists of:
	- the top window's process
	- the top window's title
	- the number of key-down events
	- the mouse-move distance

If the mouse- and keyboard-intensities are simultaneously very low, then the
SNAPSHOT is considered COLD. Otherwise we say it is WARM.

If snapshots are COLD for 30 seconds, then we consider the user to be still in
front of the computer, but passive (e.g. reading or thinking). We call him
PASSIVE.

	TODO: Do we really need the PASSIVE status?

If snapshots are COLD for 2 minutes, then we consider the user to be AWAY of the
computer.

	Rationale:
	1) I checked that 30 seconds is the time I need to read most emails.

	2) I checked that I need about 2 minutes to read one page of an
	   online article without scrolling (tested with Wired, screen 1600x1200).

Snapshots are registered only when the user is ACTIVE or PASSIVE. When the user
becomes AWAY, he is considered away from the end of the last WARM SNAPSHOT and
the SNAPSHOTS that appear after that are deleted (but this does not cross the
boundaries of SUMMARIES - see below).

	TODO: Make it independent of the summary boundaries.

Every 5 minutes the collected SNAPSHOTS are compiled into a single SUMMARY
of user activity.

	Rationale:
	1) GTD says that we should only track tasks that take at least 2 minutes.
	2) I have been creating timesheets with 5-minute accuracy for several months
	   and I was happy with this grainess.
	3) 15 minutes is already large enough to be tracked with pen and paper only.

Within a SUMMARY SNASHOTS are grouped when they have the same PROCESS and nearly
the same TITLE.

	Rationale:
	   It is very common that we switch windows, but come back to the same
	   windows to continue one task. If there is the same process and the same
	   title within one time unit, it is almost sure that this is belongs to the
	   same task.

Two TITLES are considered nearly the same, if they are equal with an exception
of at maximum 3 characters within a coherent sequence.

	Rationale:
	1) Many applications use '*' mark to indicate that a document is unsaved.
	2) Titles of the Postbox email client can differ with up to 3 characters
	   denoting the number of unread messages.
	3) Titles of Visual Studio can differ with "(Running)" and "(Debugging)"
	   phrases, which counts to 6, but these are already different things.
