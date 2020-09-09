'use strict';

const http = require('http')
const urllib = require('url')
const request = require('request')
const querystring = require('querystring')
const fs = require('fs')
const {parseCla} = require('./parser')
// const {execFileSync} = require('child_process')

var server = http.createServer()
// const subscs = []
const cachedUsers = []
const dbusers = loadUsers()
var clatpl = null

function loadUsers() {
    let users = new Map()
    if (fs.existsSync('db/users')){
        if (fs.existsSync('db/users/db')){
            let data = fs.readFileSync('db/users/db', 'utf-8')
            let us = data.split('\n')
            for (const e of us){
                if (e.length === 0)
                    continue
                let [id, name, sid] = e.split(',')
                let u = {'id': id, 'name': name, 'sid': sid}
                if (fs.existsSync('db/users/'+id)){
                    const userData = fs.readFileSync('db/users/'+id)
                    u.index = cachedUsers.length
                    cachedUsers.push(JSON.parse(userData))
                }
                users.set(id, u)
            }
        }
    } else {
        fs.mkdirSync('db/users',{recursive: true})
    }
    return users
}

function loadClaTpl(tplname) {
    if (fs.existsSync(tplname)) {
        let data = fs.readFileSync(tplname).toString()
        let result = parseCla(data)
        return result
    } else {
        return null
    }
}

function parseVm(data){
    let buf = Buffer.from(data, 'base64')
    return JSON.parse(buf.toString())
}

function parseTr(data) {
    let t = urllib.parse('trojan://'+data)
    let trobj = {}
    trobj['id'] = t.auth
    trobj['hostname'] = t.hostname
    trobj['port'] = t.port
    trobj['query'] = t.query
    trobj['ps'] = decodeURI(t.hash.substring(1))
    return trobj
}

function parseData(data) {
    let result
    let outs = []
    if (data.indexOf('proxy-groups:') !== -1){
        // schema = 'clash'
        result = parseCla(data, 'proxies')
        if (result != null && result.proxies != null) {
            for (let proxy of result.proxies) {
                outs.push({type: 'clash', value: proxy})
            }
        } else {
            outs.push({type:'unknown', value: {error: 'no proxies'}})
        }
        return outs
    } 
    let buf = Buffer.from(data, 'base64')
    let decstr = buf.toString()
    let items = decstr.split('\n')
    
    for (const e of items) {
        if (e.length === 0)
            continue
        let ind = e.indexOf(':')
        let schema = e.substring(0,ind).toLowerCase()
        let rest = e.substring(ind + 3)
        // let result
        if (schema === 'vmess'){
            result = parseVm(rest)
        } else if (schema === 'trojan') {
            result = parseTr(rest)
        } else{
            result = rest
        }
        outs.push({type: schema, value: result})
    }
    return outs
}

function encodeVm(e) {
    let stre = JSON.stringify(e.value)
    let buf = Buffer.from(stre).toString('base64')
    // buf = Buffer.from(e.type+'://'+buf).toString('base64')
    return e.type+'://'+buf
}

function encodeTr(e) {
    const stre = `${e.type}://${e.value.id}@${e.value.hostname}:${e.value.port}?${e.value.query}#${encodeURI(e.value.ps)}`
    return stre
}

function encodeCla(clas) {
    if (clatpl === null){
        clatpl = loadClaTpl('tpl.yml');
        if (clatpl === null)
            return '';
    }
    let {groups, rules} = clatpl
    let items = []
    for (const e of clas)
        items.push(e.value);
    let proxyNames = []
    let finalTpl = 'proxies:\n'
    for (const item of items) {
        proxyNames.push('- '+item.name)
        let i = 0
        for (const key in item) {
            if (i===0)
                finalTpl += '  - ';
            else
                finalTpl += '    ';
            i += 1
            finalTpl += `${key}: ${item[key]}\n`
        }
    }
    finalTpl += 'proxy-groups:\n'
    for (const group of groups) {
        let i=0
        for (const key in group) {
            if (i === 0) 
                finalTpl += '  - ';
            else
                finalTpl += '    ';
            i += 1
            if (group[key] instanceof Array) {
                finalTpl += key+':\n'
                for (const e of group[key]) 
                    finalTpl += '      '+e+'\n';
                if (group.name === 'PROXY' && key === 'proxies'){
                    for (const e of proxyNames)
                        finalTpl += '      '+e+'\n';
                }
            } else {
                finalTpl += `${key}: ${group[key]}\n`
            }
        }
    }
    finalTpl += 'rules:\n'
    for (const e of rules) {
        finalTpl += e+'\n'
    }
    return finalTpl
}

function download(url) {
    return new Promise((resolve, reject) => {
        request(url, function(err, response, body) {
            if (!err && response.statusCode === 200){
                // console.log(body)
                resolve(body)
            } else {
                if (err)
                    reject(err)
                else
                    reject(new Error('status: '+response.statusCode))
            }
        })
    })
}

function dealPost (req) {
    return new Promise((resolve, reject) => {
        let t = ''
        req.on('data', (chunk) => {
            t += chunk
        })
        req.on('error', (err) => {
            reject(err)
        })
        req.on('end', () => {
            resolve(t)
        })
    })
}

function encode (items) {
    if(items.length === 0){
        return ''
    }
    if (items[0].type === 'clash') {
        return encodeCla(items)
    }
    let composels = []
    for (const item of items) {
        if (item.type === 'vmess')
            composels.push(encodeVm(item))
        else if (item.type === 'trojan')
            composels.push(encodeTr(item))
    }
    return Buffer.from(composels.join('\n')).toString('base64')
}

function saveUserDB () {
    let userS = []
    for (const item of dbusers.entries()){
        const val = item[1]
        if (val !== null)
            userS.push(`${val.id},${val.name},${val.sid}`)
    }
    fs.writeFile('db/users/db', userS.join('\n'), (err) => {
        if (err) {throw err}
    })
}

function saveAll() {
    for (const u of cachedUsers) {
        fs.writeFile(`db/users/${u.id}`, JSON.stringify(u), (err) => {
            if (err) { throw err }
        })
    }
    saveUserDB()
    console.log('all saved')
}

function removeUser(id) {
    const us = dbusers.get(id)
    if (us != null) {
        dbusers.set(id, null)
        cachedUsers.splice(us.index, 1)
        if (fs.existsSync('db/users/'+us.id)){
            fs.unlink('db/users/'+us.id, (err) => {
                if (err) { throw err}
            })
        }
        if (fs.existsSync(`db/users/${us.sid}`)){
            fs.unlink(`db/users/${us.sid}`, (err) => {
                if (err) { throw err}
            })
        }
        // saveUserDB()
    }
}

server.on('request', async function (req, res) {
    // console.log(req.url)
    const parts = urllib.parse(req.url, true)
    // console.log(parts)
    var resData = []
    if (req.method === 'OPTIONS') {
        res.setHeader('Access-Control-Allow-Origin', '*')
        res.setHeader('Access-Control-Allow-Methods', 'GET, POST')
        res.setHeader('Access-Control-Allow-Headers', req.headers["access-control-request-headers"])
        res.writeHead(200, {'Content-Type': 'text/plain'})
        res.end()
        return
    }
    const pathname = parts.pathname
    if (pathname === '/update'){
        let url
        if (req.method === 'GET') {
            let params = parts.query
            url = params.url
        } else if (req.method === 'POST') {
            const result = await dealPost(req)
            let paramobj = JSON.parse(result)
            // let paramobj = querystring.parse(t)
            let params = []
            for (const key in paramobj) {
                if (key !== 'url')
                    params.push(key + '=' + paramobj[key])
            }
            url = paramobj.url+(params.length>0?('?'+params.join('&')):'')
        }  
        if (url != null) {
            let content = await download(url)
            resData = parseData(content)
        }
    }
    else if (pathname === '/uploadUser') {
        if (req.method === 'POST') {
            const data = await dealPost(req)
            const user = JSON.parse(data)
            let us = dbusers.get(user.id)
            if (us == null) {
                us = {'id': user.id, 'name': user.name, 'sid': user.subscId, 'index': cachedUsers.length}
                dbusers.set(us.id, us)
                // saveUserDB()
            } else {
                us.name = user.name
                us.sid = user.subscId
                // cachedUsers[us.index] = user
            }
            let encs = encode(user.selectedItems)  
            if (encs.length > 0) {
                fs.writeFile(`db/users/${us.sid}`, encs, (err) => {
                    if (err) {
                        throw err
                    }
                    // console.log(us.name+'-'+us.sid+' saved')
                })
            }          
            user.encStr = encs
            cachedUsers[us.index] = user
            // fs.writeFileSync(`db/users/${us.id}`, JSON.stringify(user))
            // console.log(us.name+'-'+us.id+' saved')
            resData = encs
        }
    } else if (pathname === '/users') {
        resData =  cachedUsers

    } else if (pathname === '/removeUser') {
        if (req.method === 'GET') {
            removeUser(parts.query.id)
            resData = 'ok'
        }
    } else if (pathname === '/saveAll') {
        // console.log("saveAll")
        saveAll()
    }  else {
        res.writeHead(200, {'Content-Type': 'text/html; charset=utf-8'})
        res.end("<center><h2>404 Not Found</h2></center><hr>")
        return
    }
    // else if (pathname === '/upload2Serv') {
    //     let id = parts.query.id
    //     let u = dbusers.get(id)
    //     if (u != null) {
    //         const output = execFileSync('./db/users/upload.bat',[`db/users/${u.name}-${u.sid}`])
    //         console.log(output)
    //     }
    // }
    res.setHeader('Access-Control-Allow-Origin', '*')
    // res.setHeader('Access-Control-Allow-Credentials', 'true')
    res.writeHead(200, {'Content-Type': 'application/json; charset=utf-8'})
    res.end(JSON.stringify(resData))
})

// server.on('close', () => {
//     console.log('server closed')    
// })

// server.on('error', (err) => {
//     console.log(err)
// })

server.listen(8000, () => {
    console.log('server is listening on 8000...')
})

// process.on('beforeExit',(code) => {
//     console.log('process exit, code: '+code)
// })