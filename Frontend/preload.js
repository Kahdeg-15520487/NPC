const { contextBridge, ipcRenderer } = require('electron')

contextBridge.exposeInMainWorld(
  'darkMode', {
  toggle: () => ipcRenderer.invoke('dark-mode:toggle')
})
contextBridge.exposeInMainWorld(
  'file', {
  send: () => ipcRenderer.send('requestFileDialog'),
  receive: (channel, func) => {
    let validChannels = ["fromMain"];
    if (validChannels.includes(channel)) {
      // Deliberately strip event as it includes `sender` 
      ipcRenderer.on(channel, (event, ...args) => func(...args));
    }
  }
})
contextBridge.exposeInMainWorld(
  'npc', {
  compile: async (arg) => {
    return await ipcRenderer.invoke('npc-compile', arg);
  },
  beautify: async (arg) => {
    return await ipcRenderer.invoke('npc-beautify', arg);
  },
  run: async (arg) => {
    return await ipcRenderer.invoke('npc-run', arg);
  }
})
