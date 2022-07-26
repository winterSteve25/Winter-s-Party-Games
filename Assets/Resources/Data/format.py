with open("Verbs.txt", "a") as newFile:
    with open("raw_verbs.txt") as f:
        lines = f.readlines()
        for line in lines:
            words = line.split()
            ingWord = words[words.__len__() - 1]
            newFile.write(ingWord.title()+"\n")