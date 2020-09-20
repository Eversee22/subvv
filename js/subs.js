Vue.filter('date', time => moment(time).format('DD/MM/YY, HH:mm'))

const baseUrl = "http://localhost:8000"
const chars = '0123456789abcdefghijklmnopqrstuvwxyz'

function generateId(len) {
    let result = '';
    let clen = chars.length
    for (let i=len; i>0; --i) {
        result += chars[Math.floor(Math.random()*clen)]
    }
    return result
}

function parseUrl(url) {
    let ind = url.indexOf('?')
    if (ind === -1)
        return {'url': url}
    let rawurl = url.substring(0,ind)
    let params = url.substring(ind+1)
    let paramobj = {'url': rawurl}
    params = params.split('&')
    for (const param of params) {
        let [k, v] = param.split('=')
        paramobj[k] = v
    }
    return paramobj
}

// function postOptions(obj) {
//     return {
//         method: 'POST',
//         mode: 'cors',
//         headers: {
//             'Content-Type': 'application/json'
//         },
//         body: JSON.stringify(obj)
//     }
// }

async function fetchData (url, options) {
    const response = await fetch(`${baseUrl}${url}`, options)
    if (response.ok) {
        const data = await response.json()
        return data
    } else {
        const msg = await response.text()
        console.log(msg)
        return null
    }
}

async function post (url, options) {
    const finalOptions = Object.assign({}, {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json'
        }
    }, options)
    const data = await fetchData(url, finalOptions)
    return data
}

async function get(url, options) {
    const finalOptions = Object.assign({}, {
        method: 'GET', 
        headers: {'Content-Type': 'text/plain'}
    }, options)
    const data = await fetchData(url, finalOptions)
    return data
}

// window.onbeforeunload = function () {
//     // console.log('leave')
//     // if (confirm('Are you sure you want to leave this page?'))
//     get('/saveAll')
// }
// window.onunload = function () {
//     console.log('enter')
// }

// window.addEventListener('resize', () => {
//     console.log(window.innerWidth, window.innerHeight)
// })
// window.addEventListener('onbeforeunload', () => {
//     confirm('Are you sure you want to leave this page?')
// })

new Vue({
    name: 'subvv',
    el: '#subvv',

    data() {
        return {
            subscs: JSON.parse(localStorage.getItem('subscs')) || [],
            selectedId: localStorage.getItem('selected-id') || null,
            selectedItems: [],
            users: [],
            selectedUserId: localStorage.getItem('selected-userid') || null,
            // selectedItem: null
            // info: ''
        }
    },

    created() {
        this.fetchUsers()
        for (const subsc of this.subscs) {
            if (subsc.content != null) {
                for (const e of subsc.content) {
                    if (e.selected)
                        this.selectedItems.push(e)
                }
            }
        }
        
    },

    // mounted() {
    //     // window.addEventListener('beforeunload', function () {
    //     //     get('/saveAll')
    //     // })
    //     // console.log('mounted')
    // },
    // beforeDestroy() {
    //     console.log('beforeDestroy')
    //     get('/saveAll')
    // },

    // destroyed() {
    //     console.log('destroyed')
    //     // get('/saveAll')
    //     // window.removeEventListener('beforeunload', e => this.beforeunloadFn(e))
    // },

    watch: {
        subscs: {
            handler: 'saveSubscs',
            deep: true
        },
        selectedId(val) {
            localStorage.setItem('selected-id', val)
        },
        selectedUserId(val) {
            localStorage.setItem('selected-userid', val)
        }
        
    },

    computed: {
        addButtonTitle () {
            return this.subscs.length + ' subscription(s) already'
        },
        sortedSubscs() {
            return this.subscs.slice().sort((a,b) => a.created - b.created)
            .sort((a,b) => (a.marked == b.marked) ? 0 : a.marked ? -1 : 1)
        },
        selectedSubsc() {
            return this.subscs.find(subsc => subsc.id == this.selectedId)
        },
        subscItemsCount() {
            if (this.selectedSubsc){
                if (this.selectedSubsc.content != null)
                    return this.selectedSubsc.content.length
                else 
                    return 0
            }   
        },
        addUserButtonTitle() {
            return this.users.length+ ' user(s) already'
        },
        selectedUser() {
            return this.users.find(user => user.id == this.selectedUserId)
        },
        selectedUserUuid() {
            const items = this.selectedUser.selectedItems
            if (items.length === 0)
                return null
            else 
                return items[0].value.id
        }  
    },

    methods: {
        // beforeunloadFn(e) {
        //     if (confirm('Are you sure you want to leave this page?'))
        //         fetchData('/saveAll', {method: 'GET', headers: { 'Content-Type': 'text/plain'}})
        // },

        addSubsc() {
            const time = Date.now()
            const subsc = {
                id: String(time),
                title: 'New subscription '+ (this.subscs.length + 1),
                url: '',
                created: time,
                updated: time,
                marked: false,
                content: null
            }
            this.subscs.push(subsc)
        },
        addUser() {
            const time = Date.now()
            const user = {
                id: String(time),
                name: 'New user '+(this.users.length + 1),
                subscId: generateId(16),
                created: time,
                selectedItems: [],
                encStr: ''
            }
            this.users.push(user)
        },
        selectUser(user) {
            this.selectedUserId = user.id
        },
        selectSubsc(subsc) {
            this.selectedId = subsc.id
        },
        saveSubscs() {
            localStorage.setItem('subscs', JSON.stringify(this.subscs))
        },
        markSubsc() {
            const subsc = this.selectedSubsc
            subsc.marked ^= true
            if (subsc.content == null)
                return
            for (const e of subsc.content){
                if (subsc.marked) {
                    if (!e.selected)
                        this.selectedItems.push(e)
                } else {
                    if (e.selected)
                        this.removeSubscItem(e)
                }
                e.selected = subsc.marked
            }                
        },
        removeSubsc() {
            if(this.selectedSubsc && confirm('Are you sure you want to delete this Subscription?')){
                const subsc = this.selectedSubsc
                const index = this.subscs.indexOf(subsc)
                if (index !== -1) {
                    this.subscs.splice(index, 1)
                    if (subsc.content == null)
                        return
                    for (const e of subsc.content){
                        e.selected = false
                        this.removeSubscItem(e)
                    }
                }
            }
        },
        async removeUser() {
            if (this.selectedUser && confirm('Are you sure you want to delete this User?')) {
                const user = this.selectedUser
                const index = this.users.indexOf(user)
                if (index != -1) {
                    this.users.splice(index, 1)
                    get(`/removeUser?id=${user.id}`)
                }
            }
        },
        removeSubscItem(item) {
            const index = this.selectedItems.indexOf(item)
            if (index !== -1) {
                   this.selectedItems.splice(index, 1)
            }
        },
        markSubscItem(item) {
            item.selected ^= true
            if (item.selected) {
                this.selectedItems.push(item)
            } else {
                this.removeSubscItem(item)
            }
        },
        addSubscItem(item) {
            if (this.selectedUser != null)
                this.selectedUser.selectedItems.push(JSON.parse(JSON.stringify(item)))
        },
        markUserSubscItem(item) {
            item.selected ^= true
            const selectedItems = this.selectedUser.selectedItems
            if (item.selected){
                selectedItems.push(item)
            } else {
                const index = selectedItems.indexOf(item)
                if (index !== -1)
                    selectedItems.splice(index, 1)
            }
        },
        // restoreUserSubscs() {
        //     this.selectedUser.selectedItems = JSON.parse(JSON.stringify(this.selectedItems))
        // },
        async updateSubsc() {
            const subsc = this.selectedSubsc
            if (!subsc.url){
                return
            } 
            const paramobj = parseUrl(subsc.url)
            // console.log(paramobj)
            // const options = postOptions(paramobj)
            const data = await post('/update', {body: JSON.stringify(paramobj)})
            if (data != null) {
                for (const e of data){
                    e.selected = subsc.marked
                }
                subsc.content = data
                subsc.updated = Date.now()
            }
        },
        // async encodeUserItems() {
        //     const user = this.selectedUser
        //     if (user.selectedItems.length === 0)
        //         return
        //     let obj = {
        //         'name': user.name, 
        //         'id': user.id, 
        //         'sid': user.subscId,
        //         'items': user.selectedItems
        //     }
        //     const options = postOptions(obj)
        //     const data = await fetchData('/encode', options)
        //     // console.log(data)
        //     user.encStr = data
        // },
        async upload() {
            const user = this.selectedUser
            // const options = postOptions(user)
            let data = await post('/uploadUser', {body: JSON.stringify(user)})
            // console.log('upload status:', status)
            user.encStr =  data
        },
        async fetchUsers() {
            this.users = await get('/users', {})
        },
        saveAll() {
            get('/saveAll')
        },
        async upload2Serv() {
            // await get(`/upload2Serv?id=${this.selectedUser.id}`)
        },
        onUserIdChange(event) {
            const id = event.currentTarget.value
            // console.log('user id changed: '+id)
            const items = this.selectedUser.selectedItems
            for (const item of items) {
                item.value.id = id
            }
        },
        generateSid() {
            this.selectedUser.subscId = generateId(16)
        }
    }
    
})
