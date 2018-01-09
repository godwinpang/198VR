import sys
import tty
import termios
import time
import re
import csv


def getch():
    fd = sys.stdin.fileno()
    old_settings = termios.tcgetattr(fd)
    try:
        tty.setraw(sys.stdin.fileno())
        ch = sys.stdin.read(1)
    finally:
        termios.tcsetattr(fd, termios.TCSADRAIN, old_settings)
    return ch


def secondsToTimeString(seconds):
    return "%02d:%02d" % (seconds//60, seconds % 60)


def timeStringToSeconds(timestr):
    if re.match("^\d+$", timestr) is not None:
        return int(timestr)

    if re.match("^\d+:\d{2}$", timestr) is None:
        return None

    minutes = int(timestr[:-3])
    seconds = int(timestr[-2:])
    return minutes * 60 + seconds


# ---------- PROGRAM ----------
def main():

    if len(sys.argv) < 2:
        print "Please enter a file to write to as a command line arg."
        print "-s to save as seconds, -b to save as 'MM:SS,seconds' (csv)"
        return

    filename = sys.argv[1]
    csvfile = open(filename, 'wb')
    csvwriter = csv.writer(csvfile)

    saveInSeconds = False
    saveInMMSS = True
    if len(sys.argv) == 3:
        if sys.argv[2] == "-s":
            saveInSeconds = True
            saveInMMSS = False
        if sys.argv[2] == "-b":
            saveInSeconds = True

    start_offset = None
    while (start_offset is None):
        print "Enter a time offset (MM:SS), or (seconds)"
        response = raw_input("--> ")

        start_offset = timeStringToSeconds(response)
        if (start_offset is None):
            print "Invalid time format."

    print "Timer will begin at {} once you press SPACE.".format(
        secondsToTimeString(start_offset))
    print "Press SPACE to log a time. Press 'x' to quit."
    startTime = None
    while True:
        ch = getch()
        if ch == 'x':
            csvfile.close()
            return

        if ord(ch) == 32:
            if startTime is None:
                print "Timer started at %s" % secondsToTimeString(start_offset)
                startTime = time.time()

            loggedTime = time.time() - startTime + start_offset
            timestamp = secondsToTimeString(loggedTime)

            output = []
            if saveInMMSS:
                output.append(timestamp)
            if saveInSeconds:
                output.append(int(loggedTime))
            print output
            csvwriter.writerow(output)


if __name__ == "__main__":
    main()
