document.getElementById('toggle-dark-mode').addEventListener('click', async () => {
    const isDarkMode = await window.darkMode.toggle();
    monaco.editor.setTheme(isDarkMode ? 'npc-dark' : 'npc');
    
})

document.getElementById('btnLoadFile').addEventListener("click", function(){
    window.file.send();
});