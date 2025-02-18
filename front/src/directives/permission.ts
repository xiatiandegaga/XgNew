import { hasPermission } from '@/hooks/web/usePermission';
import type { App, Directive, DirectiveBinding } from 'vue'

function isAuth(el:Element,binding:any,vnode){
// const { hasPermission } = usePermission();

 const value = binding.value;
 if (!value) return;
 if(!hasPermission(value)){
  el.parentNode?.removeChild(el);
 }
}

const mounted = (el:Element,binding:DirectiveBinding<any>,vnode) =>{
 isAuth(el,binding,vnode)
}

const authDirective:Directive = {
 mounted,
}

export function setupPermissionDirective(app:App) {
 app.directive('auth',authDirective)
}

export default authDirective;
