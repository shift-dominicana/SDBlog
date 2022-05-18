export const getMenuData: any[] = [  
    {
      title: 'Home',
      key: 'home',
      icon: 'fe fe-home',
      url: '/home',
    },
    {
      title: 'Configuración',
      key: 'configs',
      icon: 'fe fe-settings',
      children: [
        {
          title: 'Config. General',
          key: 'Generales',
          url: '/parametros/generales',
        },
      ],
    },
    
    {
      title: 'Mantenimientos',
      key: 'cruds',
      icon: 'fe fe-database',
      children: [
        {
          title: 'Productos',
          key: 'crud-product',
          url: '/mantenimientos/productos',
        },
        {
          title: 'Categorias de productos',
          key: 'crud-category',
          url: '/mantenimientos/prod-categorias',
        },
        {
          title: 'Tags',
          key: 'crud-tags',
          url: '/mantenimientos/tags',
        },
        {
          title: 'Users',
          key: 'crud-users',
          url: '/mantenimientos/usuarios',
        }
      ],
    },
    {
      title: 'Consultas',
      key: 'views',
      icon: 'fe fe-pie-chart',
      children: [
        {
          title: 'Usuarios',
          key: 'view-users',
          url: '/consultas/usuarios',
        },
        {
          title: 'Ordenes',
          key: 'view-orders',
          url: '/consultas/ordenes',
        }

      ],
    },
  ]
  