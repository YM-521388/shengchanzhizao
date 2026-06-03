export function useFullscreen(eleNode: string, screenMode: string) {
  let screenNode;

  //自动进入全屏
  const enterScreen = () => {
    screenNode = document.getElementById(eleNode);
    if (screenNode && screenMode === 'notfull') {
      screenNode.requestFullscreen().catch(err => {
        console.error('自动进入全屏失败:', err);
      });
      screenMode = 'isfull';
    }
  };

  //切换全屏
  const changeMode = () => {
    console.log(screenMode, 'screenMode');

    //获取screen元素，如果screenMode为notfull，则设置全屏，否则退出全屏
    if (screenMode === 'notfull' && screenNode) {
      screenNode.requestFullscreen();
      screenMode = 'isfull';
    } else {
      document.exitFullscreen();
      screenMode = 'notfull';
    }
  };

  const getScreenMode = () => {
    return screenMode;
  };
  const setScreenMode = (mode: string) => {
    screenMode = mode;
  };
  const getScreenNode = () => {
    return screenNode;
  };
  return {
    enterScreen,
    changeMode,
    getScreenMode,
    setScreenMode,
    getScreenNode,
  };
}
