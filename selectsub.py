import argparse
import getsub
import sys
import json
import base64


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
                        help='backup file of subscriptions(many,(url,b64code) each line))')

    return parser.parse_args()


if __name__ == '__main__':
    args = arg_parse()
    url = args.url
    ib64f = args.ib64f
    bakf = args.bakf
    ibak = args.ibak
    ibaku = args.ibaku
    # lbak = args.lbak

    if args.lbak:
        with open(bakf) as f:
            lines = f.readlines()
            for i, it in enumerate(lines):
                print(i, it[:it.find(',')])
        sys.exit(0)

    if url is not None:
        schemesubs = getsub.getsuburl(url, bakf)
    elif ib64f is not None:
        schemesubs = getsub.getsubfile(ib64f)
    else:
        with open(bakf) as f:
            lines = f.readlines()
            if ibaku is not None:
                schemesubs = getsub.getsuburl(lines[ibaku].strip().split(',')[0], bakf)
            else:
                schemesubs = getsub.getsubraw(lines[ibak].strip().split(',')[1])
    if "vmess" not in schemesubs:
        print("no vmess subscriptions")
        sys.exit(1)
    subs = schemesubs["vmess"]
    itemsdict = {}
    print("subs number:", len(subs))
    # sort subs via 'ps'
    subnamesi = {}
    subnames = []
    for i in range(len(subs)):
        subnamesi[subs[i]['ps']] = i
        subnames.append(subs[i]['ps'])
    subnames.sort()
    t = []
    for name in subnames:
        t.append(subs[subnamesi[name]])
    subs = t
    for i, s in enumerate(subs):
        name = s['ps']
        key = name[:2]
        if key in itemsdict.keys():
            itemsdict[key].append(name)
        else:
            itemsdict[key] = [name]
    row, col = 6, 3
    i = 0
    for k in itemsdict:
        print('--' + k + '--')
        vs = itemsdict[k]
        j = 0
        for v in vs:
            j += 1
            if j % col == 0:
                end = '\n'
            else:
                end = '  '
            print('[%d] ' % i + v, end=end)
            i += 1
        print()

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
    composels = []
    f = open('tmp', 'w', encoding='utf-8')
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
        f.write(confs + '\n')

    if len(composels) > 0:
        composition = b'\n'.join(composels)
        fncomp = base64.b64encode(composition).decode('ascii')
        with open('vtemp', 'w') as vf:
            vf.write(fncomp)
        f.write(fncomp)
    print('over')