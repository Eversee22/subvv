
def readlines(filename):
    with open(filename,encoding='utf-8') as f:
        lines = f.readlines()
        lines = [line.rstrip() for line in lines]
        lines = [line for line in lines if len(line)>0 and line.lstrip()[0]!='#']
    return lines

def read_name(line):
    k=1
    while line[k] == ' ':
        k += 1
    return line[k:]

def space_count(line):
    k=0
    while k<len(line) and line[k]==' ':
        k += 1
    return k

def readparts(j, lines):
    part={}
    parts=[]
    lenl = len(lines)
    while j<lenl:
        line = lines[j]
        spc = space_count(line)
        if spc == 0:
            if len(part) > 0:
                parts.append(part)
            # print(line)
            break
        line = line[spc:]
        if line[0] == '-':
            if len(part) > 0:
                parts.append(part)
                part={}
            line = read_name(line)
        ind = line.find(':')
        name = line[:ind].rstrip()
        value = line[ind+1:].lstrip()
        part[name]=value
        j += 1
    return j, parts
    
    
def readpgroups(j, lines):
    group={}
    groups=[]
    lenl = len(lines)
    while j<lenl:
        line = lines[j]
        spc = space_count(line)
        if spc==0:
            if len(group) > 0:
                groups.append(group)
            # print(line)
            break
        line = line[spc:]
        if line == 'proxies:':
            k=j+1
            proxies=[]
            while k<lenl:
                spc1 = space_count(lines[k])
                # print(spc1)
                line1 = lines[k][spc1:]
                if not line1.startswith('-') or spc1 < spc:
                    break
                proxies.append('- '+read_name(line1))
                k += 1
            group['proxies']=proxies
            j=k
        else:
            if line[0]=='-':
                if len(group) > 0:
                    groups.append(group)
                    group={}
                line = read_name(line)
            ind = line.find(':')
            name = line[:ind].rstrip()
            value = line[ind+1:].lstrip()
            group[name]=value
            j += 1
    return j, groups

def indexbyname(parts, dic):
    nonamei=0
    for part in parts:
        if part.get('name') is None:
            dic['noname'+str(nonamei)]=part
            nonamei += 1
        else:
            dic[part['name']]=part

def parseAll(filename):
    rest=[]
    proxies=[]
    groups=[]
    rules=[]
    i=0
    lines = readlines(filename)
    lenl = len(lines)
    while i < lenl:
        line = lines[i]
        print(line)
        if line == "proxies:":
            j, parts = readparts(i+1, lines)
            # indexbyname(parts, proxies)
            proxies = parts
            i = j
        elif line == 'proxy-groups:':
            j, parts = readpgroups(i+1, lines)
            # indexbyname(parts, groups)
            groups = parts
            i = j
        elif line == 'rules:':
            j = i+1
            while j<lenl:
                rules.append(lines[j])
                j += 1
            i = j
        else:
            rest.append(line)
            i += 1
    return (proxies, groups, rules, rest)

def parseProxies(filename):
    proxies=[]
    i=0
    lines = readlines(filename)
    lenl = len(lines)
    while i < lenl:
        line = lines[i]
        # print(line)
        if line == "proxies:":
            j, parts = readparts(i+1, lines)
            # indexbyname(parts, proxies)
            proxies = parts
            i = j
            break
        else:
            i += 1
    return proxies
    
def parseGroups(filename):
    groups=[]
    i=0
    lines = readlines(filename)
    lenl = len(lines)
    while i < lenl:
        line = lines[i]
        # print(line)
        if line == 'proxy-groups:':
            j, parts = readpgroups(i+1, lines)
            # indexbyname(parts, groups)
            groups = parts
            i = j
            break
        else:
            i += 1
    return groups
    
def parseRules(filename):
    rules=[]
    i=0
    lines = readlines(filename)
    lenl = len(lines)
    while i < lenl:
        line = lines[i]
        # print(line)
        if line == 'rules:':
            j = i+1
            while j<lenl:
                rules.append(lines[j])
                j += 1
            i = j
            break
        else:
            i += 1
    return rules

def listsubs(subs, names):
    itemsdict = {}
    print("subs number:", len(names))
    # sort subs via 'ps'
    subnamesi = {}
    subnames = []
    for i in range(len(names)):
        subnamesi[names[i]] = i
        subnames.append(names[i])
    subnames.sort()
    t = []
    for name in subnames:
        t.append(subs[subnamesi[name]])
    subs = t
    for name in subnames:
        key = name[:5]
        if itemsdict.get(key) is None:
            itemsdict[key] = [name]
        else:
            itemsdict[key].append(name)
    row, col = 6, 3
    i = 0
    keys = list(itemsdict.keys())
    keys.sort()
    for k in keys:
        print('--' + k + '--')
        vs = itemsdict[k]
        j = 0
        for v in vs:
            j += 1
            if j % col == 0:
                end = '\n'
            else:
                end = '  '
            print(('[%d] ' % i) + v, end=end)
            i += 1
        print()
    return subs

def selectsubs():
    select = input('select(ie: 0,1,2,...;0-3): ')
    slist = []
    if select.find('-') != -1:
        s, e = select.split('-')[0:2]
        for i in range(int(s), int(e), 1):
            slist.append(i)
    elif select.find(',') != -1:
        slist = [int(i) for i in select.split(',') if len(i) > 0]
    else:
        try:
            n = int(select)
            slist.append(n)
        except Exception:
            sys.exit(1)
    print(slist)
    
    return slist
    
def outPxys(parts, outf):
    outf.write('proxies:\n')
    for part in parts:
        i = 0
        for key in part:
            if i == 0:
                outf.write('  - ')
            else:
                outf.write('    ')
            i += 1
            outf.write('%s: %s\n'%(key,part[key]))

def outGroups(parts, outf):
    outf.write('proxy-groups:\n')
    for part in parts:
        i = 0
        for key in part:
            if i == 0:
                outf.write('  - ')
            else:
                outf.write('    ')
            i += 1
            if isinstance(part[key], list):
                outf.write(key+':\n')
                # print(part[key])
                for t in part[key]:
                    outf.write('      '+t+'\n')
            else:
                outf.write('%s: %s\n'%(key,part[key]))

def outRules(parts, outf):
    outf.write('rules:\n')
    for part in parts:
        outf.write(part+'\n')

if __name__=='__main__':
    import sys
    import time
    
    if len(sys.argv) == 1:
        print('usage: python %s <filename> [tempalte]' % sys.argv[0])
        sys.exit(1)
    filename=sys.argv[1]
    tmplname = 'template.yml'
    if len(sys.argv) == 3:
        tmplname = sys.argv[2]
    proxies = parseProxies(filename)
    _,groups,rules,_ = parseAll(tmplname)
    # print(groups)
    names = []
    for proxy in proxies:
        names.append(proxy['name'])
    proxies = listsubs(proxies, names)
    slist = selectsubs()
    sproxies = []
    for s in slist:
        pxy = proxies[s]
        print(pxy)
        while True:
            c = input('change any: ')
            if len(c) == 0:
                break
            if pxy.get(c) is None:
                continue
            # print('change '+c)
            value = input('value: ')
            pxy[c] = value
            print('changed '+c, pxy[c])
        sproxies.append(pxy)
    spnames = ['- '+sp['name'] for sp in sproxies]
    with open('config'+str(int(time.time()))+'.yml', 'w', encoding='utf-8') as f:
        outPxys(sproxies, f)
        for g in groups:
            g['proxies'].extend(spnames)
        # print(groups)
        outGroups(groups, f)
        outRules(rules, f)
    print('over')
    

    