import time

print("HELLO!! I am called")

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
    passedFirstDittoMark = False # Ditto mark = "
    newLine = ""
    LeftArrowSplit = line.split("<")
    ## raise exception if split result is > 2
    newLine = f"{LeftArrowSplit[0]}<"
    rightArrowSplit = LeftArrowSplit[1].split(">")

    words = rightArrowSplit[0].split(' ')
    for word in words:
        if ("name" in word):
            continue
        newLine += f"{word} "

    endSign = '>'
    if ("/>" in line):
        endSign = "\>"
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


            # save line when none matches
            newData += f"{line}\n"
        f.close()
finally:
    f.close()

for line in newData.split('\n'):
    if ("<BufferStop" in line):
        print(line)

# TODO save newData
try:
    with open('./output/Dordrecht_Merged_XSD_Validated_v2.xml', 'w') as f:
        f.write(newData)
finally:
    f.close()


## for line in newData.split("\n"):
##     print(line)
##     time.sleep(0.1)