import argparse
import getsub
import sys
import json
import base64
from urllib import parse


def arg_parse():
    parser = argparse.ArgumentParser(description='get v2ray subscription')
    parser.add_argument('-url', dest='url', help='url of subscription')
    parser.add_argument('-U', dest='ibaku', type=int, default=None,
                        help='index of backup url of subscription in "bakf"')
    parser.add_argument('-i', dest='ib64f', default=None,
                        help='input file of base64 encoding of subscription(only one)')
    parser.add_argument('-I', dest='ibak', type=int, default=-1,
                        help='index of backup "b64code" of subscription in "bakf"')
    parser.add_argument('-l', dest='lbak', action="store_true", default=False,
                        help='list subscriptions in "bakf"')
    parser.add_argument('-b', dest='bakf', default='baksubs',
                        help='backup file of subscriptions((url,b64code) each line)')
    parser.add_argument('-type', dest='type', default="vm", help='subscription type(vmess,trojan,all)')
    parser.add_argument('-conf', dest='conf', help='file of configs formatted')
    return parser.parse_args()

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
    
def writesubs(fname, composels):
    if len(composels) > 0:
        composition = b'\n'.join(composels)
        fncomp = base64.b64encode(composition).decode('ascii')
        with open(fname, 'w') as f:
            f.write(fncomp)
        tmpf.write(fncomp)

def makevmsubs2(subs, slist):
    composels = []
    pattern = {"add": "", "aid": 0, "host": "", "id": "", "net": "", "path": "", "port": 0,
               "ps": "", "tls": "", "v": 2}

    def confmap(name):
        try:
            value = sub[name]
        except KeyError:
            value = pattern[name]
        return value

    for i in slist:
        if i < 0 or i >= len(subs):
            print(i, "out range")
            continue
        sub = subs[i]
        conf = pattern.copy()
        for k in conf:
            conf[k] = confmap(k)
        print(conf)
        while True:
            cmd = input('any change: ')
            if len(cmd) == 0 or cmd not in conf:
                break
            print(conf[cmd])
            value = input('change ' + cmd + ': ')
            print(value)
            if cmd in ['aid', 'port']:
                value = int(value)
            conf[cmd] = value
            print('ok')
        confs = json.dumps(conf, ensure_ascii=False, separators=(',', ':'))  # dict to string
        composels.append(b'vmess://' + base64.b64encode(bytes(confs, 'utf-8')))
        tmpf.write(confs + '\n')
    
    return composels

def makevmpre():
    subs = schemesubs["vmess"]
    names = []
    for sub in subs:
        names.append(sub['ps'])
    subs = listsubs(subs, names)
    slist = selectsubs()
    
    return subs, slist
    
def makevmsubs():
    if schemesubs.get('vmess') is None:
       print("no vmess subscriptions")
       sys.exit(1)
    subs, slist = makevmpre()
    composels = makevmsubs2(subs, slist)
    
    writesubs('vtemp', composels)

def maketrsubs2(subs, slist):
    composels = []
    for i in slist:
        if i<0 or i>=len(subs):
            print(i, 'out range')
            continue
        composels.append(b'trojan://' + bytes(subs[i], 'utf-8'))
        tmpf.write(subs[i] + '\n')
    
    return composels

def maketrpre():
    subs = schemesubs['trojan']
    names = []
    for sub in subs:
        urlname = parse.unquote(sub[sub.find('#')+1:])
        names.append(urlname)
    subs = listsubs(subs, names)
    slist = selectsubs()
    
    return subs, slist
    
def maketrsubs():
    if schemesubs.get('trojan') is None:
        print("no trojan subscriptions")
        sys.exit(1)
    subs, slist = maketrpre()
    composels = maketrsubs2(subs, slist)
    
    writesubs('trtemp', composels)

def makesubs():
    typenames = ['vmess', 'trojan']
    composels = []
    for name in typenames:
        if name == 'vmess':
            subs, slist = makevmpre()
            comps = makevmsubs2(subs, slist)
        elif name == 'trojan':
            subs, slist = maketrpre()
            comps = maketrsubs2(subs, slist)
        composels.extend(comps)
    
    writesubs('mixtemp', composels)
    
def getsubconf(filename): 
    with open(filename, encoding='utf-8') as f:
        lines = f.readlines()
        lines = [line.strip() for line in lines]
        lines = [line for line in lines if len(line)>0 and line[0]!='#']
        bconfs = []
        for line in lines:
            i = line.find(',')
            scheme = line[:i]
            conf = line[i+1:]
            i = conf.find(',')
            name = conf[:i]
            conf = conf[i+1:]
            print(scheme, name)
            if scheme == 'tr':
                bconfs.append(b'trojan://' + bytes(conf, 'utf-8'))
            elif scheme == 'vm':
                bconfs.append(b'vmess://' + base64.b64encode(bytes(conf, 'utf-8')))
        writesubs(filename+'temp', bconfs)
                
    
if __name__ == '__main__':
    args = arg_parse()
    url = args.url
    ib64f = args.ib64f
    bakf = args.bakf
    ibak = args.ibak
    ibaku = args.ibaku
    stype = args.type
    # lbak = args.lbak
    conff = args.conf

    if args.lbak:
        with open(bakf) as f:
            lines = f.readlines()
            for i, it in enumerate(lines):
                print(i, it[:it.find(',')])
        sys.exit(0)
    
    tmpf = open('tmp', 'w', encoding='utf-8')
    
    if url is not None:
        schemesubs = getsub.getsuburl(url, bakf)
    elif conff is not None:
        getsubconf(conff)
        sys.exit(1)
    elif ib64f is not None:
        schemesubs = getsub.getsubfile(ib64f)
    else:
        with open(bakf) as f:
            lines = f.readlines()
            if ibaku is not None:
                schemesubs = getsub.getsuburl(lines[ibaku].strip().split(',')[0], bakf)
            else:
                schemesubs = getsub.getsubraw(lines[ibak].strip().split(',')[1])
    
    if stype == "vm":
        makevmsubs()
    elif stype == "tr":
        maketrsubs()
    elif stype == "all":
        makesubs()
    tmpf.close()
    print('over')