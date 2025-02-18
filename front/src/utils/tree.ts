export function getTreeList(v: any) {
    let resData: any = [];
    if (v) {
      var parentsData = v.filter(x => x.pid == "0");
      if (parentsData && parentsData.length > 0) {
        parentsData.forEach(item => {
          item.children = getChildData(v, item.id);
          resData.push(item);
        });
      }
    }
    return resData;
  }
export function getChildData(data: any, pid: string) {
  let resData: any = [];
  var childData = data.filter(x => x.pid == pid);
  if (childData && childData.length > 0) {
    childData.forEach(item => {
      item.children = getChildData(data, item.id);
      resData.push(item);
    });
  }
  return resData;
}