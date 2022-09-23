import time

def handlePassageRef(line):
    BEFORE_PASSAGE_REF = 0
    IN_PASSAGE_REF_BEGIN = 1
    IN_PASSAGE_REF_END = 2
    AFTER_PASSAGE_REF = 3

    #passageRefs
    jumper = ""
    passageRef = ""
    state = BEFORE_PASSAGE_REF
    for char in line:
        # print(f"char is: {char};\t state is: {state}")
        if (state == BEFORE_PASSAGE_REF):
            if (char == ' '):
                state = IN_PASSAGE_REF_BEGIN
                continue
            jumper += char
            continue
        if (state == IN_PASSAGE_REF_BEGIN):
            if (char == '"'):
                state = IN_PASSAGE_REF_END
            continue
        if (state == IN_PASSAGE_REF_END):
            if (char == '"'):
                state = AFTER_PASSAGE_REF
                continue
            passageRef += char
            continue
        if (state == AFTER_PASSAGE_REF):
            if (char == '/'):
                continue
            jumper += char
            continue
    # print(passageRef)

    return f"{jumper} <PassageRefs> {passageRef} </PassageRefs> </Jumper>"

def removeName(line):
    REMOVING = 0
    NORMAL = 1
    passedFirstDittoMark = False # Ditto mark = "
    newLine = ""
    LeftArrowSplit = line.split("<")
    ## raise exception if split result is > 2
    newLine = f"{LeftArrowSplit[0]}<"
    rightArrowSplit = LeftArrowSplit[1].split(">")

    words = rightArrowSplit[0].split(' ')
    state = NORMAL
    for word in words:
        if ("name" in word):
            if (word[-1] != '"'):
                state = REMOVING
            continue
        if (state == REMOVING):
            if ('"' in word):
                state = NORMAL
            continue
        newLine += f"{word} "

    endSign = '>'
    if ("/>" in line):
        endSign = "\>"

    if (len(LeftArrowSplit) > 2):
        return f"{newLine}> {rightArrowSplit[1]} <{LeftArrowSplit[2]}"
    return f"{newLine}{endSign}"


def handleBufferstop(line):
    # TODO then add bufferstop type
    newLine = ""
    for char in line:
        if (char == '>' or char == '/'):
            newLine += 'bufferstopType="fixated"'
        newLine += char
    # TODO then add add position reference
    return f"{newLine}"


try:
    # open XSD file  #, encoding="utf-8")
    with open('./Dordrecht_Merged_XSD_Validated.xml', 'r') as f:
        data = f.read()
        data = data.replace("railConnectionRef", "implementationObjectRef")
        ## print(data)

        linedata = data.split('\n')

        newData = ""
        for line in linedata:
            # then fix removal of name
            if ("name" in line):
                # print(f"old: {line}")
                line = removeName(line)
                # print(f"new: {line}")
                # time.sleep(0.01)

            ## wordData = line.split(' ')
            ## for word in wordData:
            # first change jumper and passagerefs
            if ("<Jumper " in line):
                newLine = handlePassageRef(line)
                # print(newLine)
                newData += f"{newLine}\n"
                continue

            if ("<BufferStop" in line):
                # TODO then add bufferstop type
                # TODO then add add position reference
                newLine = handleBufferstop(line)
                newData += f"{newLine}\n"
                # print(newLine)
                continue

            if ("<Metadata" in line):
                lines = line.split('/')
                newData += f'{lines[0]} isInService="Unknown" /{lines[1]}\n'
                continue
            if ("<Passage " in line):
                lines = line.split('>')
                newData += f'{lines[0]} passageSpeed="40" >\n'
                continue
            if ("<KCrossing" in line):
                lines = line.split('>')
                newData += f'{lines[0]} isMovable="Unknown" operatingType="Unknown" >\n'
                continue
            if ("<SwitchBlades" in line):
                lines = line.split('>')
                newData += f'{lines[0]} hasSwitchChecker="Unknown" operatingType="Unknown" >\n'
                continue
            if ("<SwitchMechanism" in line):
                newData += line.replace('hasSwitchChecker="Unknown"', "").replace('hasSwitchChecker="True"', "").replace('hasSwitchChecker="False"', "")
                newData += '\n'
                continue
            if ("<SingleSwitch" in line):
                newData += line.replace('isSymmetric="False"', "").replace('isSymmetric="True"',"").replace('isSymmetric="Unknown"',"").replace('divergingSpeed="40"',"").replace('divergingSpeed="50"',"").replace('divergingSpeed="60"',"").replace('divergingSpeed="80"',"")
                newData += '\n<DivergingPassageRefs />\n<Passage sideTag="Unknown" passageSpeed="40" puic="d065aa4f-4390-4201-93a8-0ba831fd95b3" > <MetaData /> <Location /> </passage>'
                newData += '\n'
                continue


            # save line when none matches
            newData += f"{line}\n"
        f.close()
finally:
    f.close()

# for line in newData.split('\n'):
#     if ("<BufferStop" in line):
#         print(line)

# TODO save newData
try:
    with open('./output/Dordrecht_Merged_XSD_Validated_v2.xml', 'w') as f:
        f.write(newData)
finally:
    f.close()


## for line in newData.split("\n"):
##     print(line)
##     time.sleep(0.1)
