
// 附件转化成base64
export function getBase64(file) {
  console.log('file:', file);
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file.raw);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });
}

