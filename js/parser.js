'use strict';

// var Parser = new Object();

function spCount(e) {
    let k=0
    while (k<e.length && e[k] === ' ')
        ++k;
    return k
}

function readSpace(e, start=0) {
    let k=start
    while (e[k] === ' ')
        ++k;
    return e.substring(k)
}

function parseProxies(index, items) {
    const itemsLen = items.length
    let j = index
    let part = {}
    let proxies =[]
    while (j < itemsLen) {
        let item1 = items[j]
        let spc = spCount(item1)
        if (spc === 0) {
            if(Object.keys(part).length > 0) {
                proxies.push(part)
            }
            break
        }
        item1 = item1.substring(spc)
        if (item1[0] === '-') {
            if (Object.keys(part).length > 0) {
                proxies.push(part)
                part = {}
            }
            item1 = readSpace(item1, 1)
        }
        let ind = item1.indexOf(':')
        let key = item1.substring(0, ind).trimEnd()
        let val = item1.substring(ind+1).trimStart()
        part[key] = val
        ++j
    }
    return {parts: proxies, index: j}
}

function parseGroups(index, items) {
    const itemsLen = items.length
    let j = index
    let part = {}
    let groups = []
    while (j<itemsLen) {
        let item = items[j]
        let spc = spCount(item)
        if (spc === 0) {
            if(Object.keys(part).length > 0) {
                groups.push(part)
            }
            break
        }
        item = item.substring(spc)
        if (item === 'proxies:'){
            let k = j+1
            let proxies = []
            while (k<itemsLen) {
                let spc1 = spCount(items[k])
                let item1 = items[k].substring(spc1)
                if (item1[0] !== '-' || spc1 < spc) {
                    break
                }
                proxies.push('- '+readSpace(item1, 1))
                k++
            }
            part['proxies'] = proxies
            j = k
        } else {
            if (item[0] === '-') {
                if (Object.keys(part).length > 0){
                    groups.push(part)
                    part = {}
                }
                item = readSpace(item, 1)
            }
            let ind = item.indexOf(':')
            let key = item.substring(0, ind).trimEnd()
            let val = item.substring(ind + 1).trimStart()
            part[key] = val
            j++
        }
    }
    return {parts: groups, index: j}
}

function parseRules (index, items) {
    const itemsLen = items.length
    let j
    let rules = []
    for (j=index; j < itemsLen; ++j)
        rules.push(items[j]);
   return {parts: rules, index: j}
}

function parseCla(data, name) {
    let items = []
    for (const e of data.split('\n')) {
        if (e.trimStart()[0] === '#')
            continue
        let t = e.trimEnd()
        if (t.length > 0)
            items.push(t)
    }
    const itemsLen = items.length

    function getPart(partName, parser) {
        for (let i=0; i<itemsLen; i++) {
            if (items[i] === partName) {
                let {parts, _} = parser(i+1, items)
                return parts
            }
        }
        return null
    }

    if (name != null) {
        let result
        if (name === 'proxies'){
            result = getPart('proxies:', parseProxies)
            return {proxies: result}
        } else if (name === 'groups') {
            result = getPart('proxy-groups:', parseGroups)
            return {groups: result}
        } else if (name === 'rules') {
            result = getPart('rules:', parseRules)
            return {rules: result}
        }
    }

    let rest = []
    let result = {}
    let i=0
    let partsName
    while (i<itemsLen){
        let item = items[i]
        // let spc = spCount(item)
        // if (spc === 0) {
            let ind = item.indexOf(':')
            if (ind !== -1)
                partsName = item.substring(0,ind)
            else
                partsName = item
            if (partsName === 'proxies') {
                let {parts, index} = parseProxies(i+1, items)
                result['proxies'] = parts
                i = index
            } else if (partsName === 'proxy-groups') {
                let {parts, index} = parseGroups(i+1, items)
                result['groups'] = parts
                i = index
            } else if (partsName === 'rules') {
                let {parts, index} = parseRules(i+1, items)
                result['rules'] = parts
                i = index
            } else {
                rest.push(item)
                i++
            }
        // } 
        // else {
        //     rest.push(item)
        //     i++
        // }
    }
    result['rest'] = rest
    return result
}

module.exports = {
    'parseCla': parseCla
    // 'parseProxies': parseProxies,
    // 'parseGroups': parseGroups
}