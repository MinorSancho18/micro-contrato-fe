let tabla;
let contratoActual = null;
let modoEdicion = false;

$(document).ready(function () {
    inicializarDataTable();
    cargarCombos();
    configurarEventos();
    cargarContratos();
});

function inicializarDataTable() {
    tabla = $('#tablaContratos').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
        },
        columns: [
            { data: 'idContrato' },
            { data: 'nombreCliente' },
            {
                data: 'fechaRecogida',
                render: function (data) {
                    return new Date(data).toLocaleString('es-CR');
                }
            },
            {
                data: 'fechaDevolucion',
                render: function (data) {
                    return new Date(data).toLocaleString('es-CR');
                }
            },
            { data: 'descripcionEstado' },
            {
                data: 'montoTotal',
                render: function (data) {
                    return '₡' + parseFloat(data).toFixed(2);
                }
            },
            {
                data: 'saldo',
                render: function (data) {
                    return '₡' + parseFloat(data).toFixed(2);
                }
            },
            {
                data: 'confirmado',
                render: function (data) {
                    return data ? '<span class="badge bg-success">Sí</span>' : '<span class="badge bg-warning">No</span>';
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-info" onclick="verDetalle(${row.idContrato})">Ver</button>
                        <button class="btn btn-sm btn-warning" onclick="editarContrato(${row.idContrato})">Editar</button>
                    `;
                }
            }
        ]
    });
}

function configurarEventos() {
    $('#btnNuevoContrato').click(function () {
        modoEdicion = false;
        limpiarFormulario();
        $('#modalContratoTitulo').text('Nuevo Contrato');
        $('#modalContrato').modal('show');
    });

    $('#btnGuardarContrato').click(guardarContrato);
    $('#btnConfirmarContrato').click(confirmarContrato);
    $('#btnIniciarContrato').click(iniciarContrato);
    $('#btnAgregarVehiculo').click(mostrarModalAgregarVehiculo);
    $('#btnAgregarExtra').click(mostrarModalAgregarExtra);
    $('#btnConfirmarAgregarVehiculo').click(agregarVehiculo);
    $('#btnConfirmarAgregarExtra').click(agregarExtra);

    $('#fechaRecogida, #fechaDevolucion').change(calcularDias);
}

async function cargarCombos() {
    try {
        const [clientes, sucursales, usuarios, estados] = await Promise.all([
            $.get('/Contratos/ObtenerClientes'),
            $.get('/Contratos/ObtenerSucursales'),
            $.get('/Contratos/ObtenerUsuarios'),
            $.get('/Contratos/ObtenerEstados')
        ]);

        if (clientes.success) {
            $('#idCliente').append(clientes.data.map(c =>
                `<option value="${c.idCliente}">${c.nombre} ${c.apellido} - ${c.cedula}</option>`
            ).join(''));
        }

        if (sucursales.success) {
            $('#idSucursal').append(sucursales.data.map(s =>
                `<option value="${s.idSucursal}">${s.nombre}</option>`
            ).join(''));
        }

        if (usuarios.success) {
            $('#idUsuario').append(usuarios.data.map(u =>
                `<option value="${u.idUsuario}">${u.nombre} ${u.apellido}</option>`
            ).join(''));
        }

        if (estados.success) {
            $('#idEstado').append(estados.data.map(e =>
                `<option value="${e.idEstado}">${e.descripcion}</option>`
            ).join(''));
        }
    } catch (error) {
        Swal.fire('Error', 'Error al cargar los combos', 'error');
    }
}

async function cargarContratos() {
    try {
        const response = await $.get('/Contratos/Listar');
        if (response.success) {
            tabla.clear();
            tabla.rows.add(response.data);
            tabla.draw();
        }
    } catch (error) {
        Swal.fire('Error', 'Error al cargar contratos', 'error');
    }
}

function limpiarFormulario() {
    $('#formContrato')[0].reset();
    $('#idContrato').val('');
    $('#montoTotal').val('0');
    $('#saldo').val('0');
}

async function guardarContrato() {
    if (!validarFormulario()) {
        return;
    }

    const contrato = {
        idContrato: parseInt($('#idContrato').val()) || 0,
        idCliente: parseInt($('#idCliente').val()),
        nombreCliente: '',
        fechaRecogida: $('#fechaRecogida').val(),
        fechaDevolucion: $('#fechaDevolucion').val(),
        idSucursal: parseInt($('#idSucursal').val()),
        idEstado: parseInt($('#idEstado').val()),
        descripcionEstado: '',
        idUsuario: parseInt($('#idUsuario').val()),
        montoTotal: parseFloat($('#montoTotal').val()) || 0,
        confirmado: false,
        garantiaAprobada: $('#garantiaAprobada').is(':checked'),
        saldo: parseFloat($('#saldo').val()) || 0
    };

    try {
        let response;
        if (modoEdicion) {
            response = await $.ajax({
                url: `/Contratos/Actualizar?id=${contrato.idContrato}`,
                method: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(contrato)
            });
        } else {
            response = await $.ajax({
                url: '/Contratos/Crear',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(contrato)
            });
        }

        if (response.success) {
            Swal.fire('Éxito', 'Contrato guardado correctamente', 'success');
            $('#modalContrato').modal('hide');
            cargarContratos();
        } else {
            Swal.fire('Error', response.message, 'error');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al guardar el contrato', 'error');
    }
}

function validarFormulario() {
    if (!$('#idCliente').val()) {
        Swal.fire('Advertencia', 'Debe seleccionar un cliente', 'warning');
        return false;
    }
    if (!$('#fechaRecogida').val()) {
        Swal.fire('Advertencia', 'Debe ingresar la fecha de recogida', 'warning');
        return false;
    }
    if (!$('#fechaDevolucion').val()) {
        Swal.fire('Advertencia', 'Debe ingresar la fecha de devolución', 'warning');
        return false;
    }
    if (!$('#idSucursal').val()) {
        Swal.fire('Advertencia', 'Debe seleccionar una sucursal', 'warning');
        return false;
    }
    if (!$('#idUsuario').val()) {
        Swal.fire('Advertencia', 'Debe seleccionar un usuario', 'warning');
        return false;
    }
    if (!$('#idEstado').val()) {
        Swal.fire('Advertencia', 'Debe seleccionar un estado', 'warning');
        return false;
    }

    const fechaRecogida = new Date($('#fechaRecogida').val());
    const fechaDevolucion = new Date($('#fechaDevolucion').val());
    if (fechaDevolucion <= fechaRecogida) {
        Swal.fire('Advertencia', 'La fecha de devolución debe ser posterior a la fecha de recogida', 'warning');
        return false;
    }

    return true;
}

function calcularDias() {
    const fechaRecogida = new Date($('#fechaRecogida').val());
    const fechaDevolucion = new Date($('#fechaDevolucion').val());
    
    if (fechaRecogida && fechaDevolucion && fechaDevolucion > fechaRecogida) {
        const dias = Math.ceil((fechaDevolucion - fechaRecogida) / (1000 * 60 * 60 * 24));
        console.log('Días calculados:', dias);
    }
}

function calcularDiasDeUso(fechaInicio, fechaFin) {
    const inicio = new Date(fechaInicio);
    const fin = new Date(fechaFin);
    const dias = Math.ceil((fin - inicio) / (1000 * 60 * 60 * 24));
    return dias > 0 ? dias : 1;
}

async function editarContrato(id) {
    try {
        const response = await $.get(`/Contratos/Obtener?id=${id}`);
        if (response.success) {
            modoEdicion = true;
            const contrato = response.data;
            
            $('#idContrato').val(contrato.idContrato);
            $('#idCliente').val(contrato.idCliente);
            $('#fechaRecogida').val(new Date(contrato.fechaRecogida).toISOString().slice(0, 16));
            $('#fechaDevolucion').val(new Date(contrato.fechaDevolucion).toISOString().slice(0, 16));
            $('#idSucursal').val(contrato.idSucursal);
            $('#idEstado').val(contrato.idEstado);
            $('#idUsuario').val(contrato.idUsuario);
            $('#montoTotal').val(contrato.montoTotal);
            $('#saldo').val(contrato.saldo);
            $('#garantiaAprobada').prop('checked', contrato.garantiaAprobada);

            $('#modalContratoTitulo').text('Editar Contrato');
            $('#modalContrato').modal('show');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al cargar el contrato', 'error');
    }
}

async function verDetalle(id) {
    try {
        const response = await $.get(`/Contratos/ObtenerDetalle?id=${id}`);
        if (response.success) {
            contratoActual = response.data;
            mostrarDetalle(contratoActual);
            $('#modalDetalle').modal('show');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al cargar el detalle', 'error');
    }
}

function mostrarDetalle(detalle) {
    const contrato = detalle.contrato;
    
    $('#detalleIdContrato').text(contrato.idContrato);
    $('#detalleCliente').text(contrato.nombreCliente);
    $('#detalleFechaRecogida').text(new Date(contrato.fechaRecogida).toLocaleString('es-CR'));
    $('#detalleFechaDevolucion').text(new Date(contrato.fechaDevolucion).toLocaleString('es-CR'));
    $('#detalleSucursal').text(contrato.idSucursal);
    $('#detalleUsuario').text(contrato.idUsuario);
    $('#detalleEstado').text(contrato.descripcionEstado);
    $('#detalleMontoTotal').text('₡' + parseFloat(contrato.montoTotal).toFixed(2));
    $('#detalleSaldo').text('₡' + parseFloat(contrato.saldo).toFixed(2));
    $('#detalleConfirmado').html(contrato.confirmado ? '<span class="badge bg-success">Sí</span>' : '<span class="badge bg-warning">No</span>');
    $('#detalleGarantia').html(contrato.garantiaAprobada ? '<span class="badge bg-success">Sí</span>' : '<span class="badge bg-warning">No</span>');

    const tbodyVehiculos = $('#tablaVehiculos tbody');
    tbodyVehiculos.empty();
    detalle.vehiculos.forEach(v => {
        const btnInspeccionar = !v.inspeccionado
            ? `<button class="btn btn-xs btn-primary" onclick="marcarInspeccionado(${v.idVehiculoContrato})">Inspeccionar</button>`
            : '<span class="badge bg-success">Inspeccionado</span>';
        
        tbodyVehiculos.append(`
            <tr>
                <td>${v.descripcionVehiculo}</td>
                <td>${v.diasDeUso}</td>
                <td>₡${parseFloat(v.costoDiario).toFixed(2)}</td>
                <td>₡${parseFloat(v.subtotal).toFixed(2)}</td>
                <td>${v.inspeccionado ? 'Sí' : 'No'}</td>
                <td>${btnInspeccionar}</td>
            </tr>
        `);
    });

    const tbodyExtras = $('#tablaExtras tbody');
    tbodyExtras.empty();
    detalle.extras.forEach(e => {
        tbodyExtras.append(`
            <tr>
                <td>${e.descripcionExtra}</td>
                <td>${e.diasDeUso}</td>
                <td>₡${parseFloat(e.costoDiario).toFixed(2)}</td>
                <td>₡${parseFloat(e.subtotal).toFixed(2)}</td>
            </tr>
        `);
    });

    configurarBotonesDetalle(contrato);
}

function configurarBotonesDetalle(contrato) {
    const puedeAgregarVehiculos = !contrato.confirmado;
    const puedeConfirmar = !contrato.confirmado;
    const puedeIniciar = contrato.confirmado && contrato.idEstado === 2; // Estado "Reservado"

    $('#btnAgregarVehiculo').prop('disabled', !puedeAgregarVehiculos);
    $('#btnAgregarExtra').prop('disabled', !puedeAgregarVehiculos);
    $('#btnConfirmarContrato').prop('disabled', !puedeConfirmar);
    $('#btnIniciarContrato').prop('disabled', !puedeIniciar);
}

let vehiculosDisponibles = [];
let extrasDisponibles = [];

async function mostrarModalAgregarVehiculo() {
    try {
        const response = await $.get('/Contratos/ObtenerVehiculos');
        if (response.success) {
            vehiculosDisponibles = response.data;
            $('#selectVehiculo').html('<option value="">Seleccione...</option>');
            $('#selectVehiculo').append(response.data.map(v =>
                `<option value="${v.idVehiculo}" data-descripcion="${v.descripcion}" data-costo="${v.costo}">${v.descripcion} (₡${v.costo})</option>`
            ).join(''));
            $('#modalAgregarVehiculo').modal('show');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al cargar vehículos', 'error');
    }
}

async function agregarVehiculo() {
    const idVehiculo = $('#selectVehiculo').val();
    if (!idVehiculo) {
        Swal.fire('Advertencia', 'Debe seleccionar un vehículo', 'warning');
        return;
    }

    const vehiculo = vehiculosDisponibles.find(v => v.idVehiculo == idVehiculo);
    if (!vehiculo) {
        Swal.fire('Error', 'Vehículo no encontrado', 'error');
        return;
    }

    const diasDeUso = calcularDiasDeUso(contratoActual.contrato.fechaRecogida, contratoActual.contrato.fechaDevolucion);

    try {
        const response = await $.ajax({
            url: `/Contratos/AgregarVehiculo`,
            method: 'POST',
            data: {
                idContrato: contratoActual.contrato.idContrato,
                idVehiculo: idVehiculo,
                descripcionVehiculo: vehiculo.descripcion,
                diasDeUso: diasDeUso,
                costoDiario: vehiculo.costo
            }
        });

        if (response.success) {
            Swal.fire('Éxito', 'Vehículo agregado correctamente', 'success');
            $('#modalAgregarVehiculo').modal('hide');
            verDetalle(contratoActual.contrato.idContrato);
        } else {
            Swal.fire('Error', response.message, 'error');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al agregar vehículo', 'error');
    }
}

async function mostrarModalAgregarExtra() {
    try {
        const response = await $.get('/Contratos/ObtenerExtras');
        if (response.success) {
            extrasDisponibles = response.data;
            $('#selectExtra').html('<option value="">Seleccione...</option>');
            $('#selectExtra').append(response.data.map(e =>
                `<option value="${e.idExtra}" data-descripcion="${e.descripcion}" data-costo="${e.costo}">${e.descripcion} (₡${e.costo})</option>`
            ).join(''));
            $('#diasDeUsoExtra').val(1);
            $('#modalAgregarExtra').modal('show');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al cargar extras', 'error');
    }
}

async function agregarExtra() {
    const idExtra = $('#selectExtra').val();
    const diasDeUso = parseInt($('#diasDeUsoExtra').val());
    
    if (!idExtra) {
        Swal.fire('Advertencia', 'Debe seleccionar un extra', 'warning');
        return;
    }

    if (!diasDeUso || diasDeUso < 1) {
        Swal.fire('Advertencia', 'Días de uso debe ser mayor a 0', 'warning');
        return;
    }

    const extra = extrasDisponibles.find(e => e.idExtra == idExtra);
    if (!extra) {
        Swal.fire('Error', 'Extra no encontrado', 'error');
        return;
    }

    try {
        const response = await $.ajax({
            url: `/Contratos/AgregarExtra`,
            method: 'POST',
            data: {
                idContrato: contratoActual.contrato.idContrato,
                idExtra: idExtra,
                descripcionExtra: extra.descripcion,
                diasDeUso: diasDeUso,
                costoDiario: extra.costo
            }
        });

        if (response.success) {
            Swal.fire('Éxito', 'Extra agregado correctamente', 'success');
            $('#modalAgregarExtra').modal('hide');
            verDetalle(contratoActual.contrato.idContrato);
        } else {
            Swal.fire('Error', response.message, 'error');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al agregar extra', 'error');
    }
}

async function marcarInspeccionado(idVehiculoContrato) {
    const idUsuario = contratoActual.contrato.idUsuario;
    
    try {
        const response = await $.ajax({
            url: `/Contratos/MarcarVehiculoInspeccionado`,
            method: 'PUT',
            data: {
                idVehiculoContrato: idVehiculoContrato,
                idUsuario: idUsuario
            }
        });

        if (response.success) {
            Swal.fire('Éxito', 'Vehículo marcado como inspeccionado', 'success');
            verDetalle(contratoActual.contrato.idContrato);
        } else {
            Swal.fire('Error', response.message, 'error');
        }
    } catch (error) {
        Swal.fire('Error', 'Error al marcar vehículo', 'error');
    }
}

async function confirmarContrato() {
    const result = await Swal.fire({
        title: '¿Confirmar contrato?',
        text: 'Esta acción no se puede deshacer',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, confirmar',
        cancelButtonText: 'Cancelar'
    });

    if (result.isConfirmed) {
        try {
            const response = await $.ajax({
                url: `/Contratos/Confirmar`,
                method: 'PUT',
                data: {
                    id: contratoActual.contrato.idContrato,
                    idUsuario: contratoActual.contrato.idUsuario
                }
            });

            if (response.success) {
                Swal.fire('Éxito', 'Contrato confirmado', 'success');
                verDetalle(contratoActual.contrato.idContrato);
                cargarContratos();
            } else {
                Swal.fire('Error', response.message, 'error');
            }
        } catch (error) {
            Swal.fire('Error', 'Error al confirmar contrato', 'error');
        }
    }
}

async function iniciarContrato() {
    const result = await Swal.fire({
        title: '¿Iniciar contrato?',
        text: 'El contrato pasará a estado En Progreso',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, iniciar',
        cancelButtonText: 'Cancelar'
    });

    if (result.isConfirmed) {
        try {
            const response = await $.ajax({
                url: `/Contratos/Iniciar`,
                method: 'PUT',
                data: {
                    id: contratoActual.contrato.idContrato,
                    idUsuario: contratoActual.contrato.idUsuario
                }
            });

            if (response.success) {
                Swal.fire('Éxito', 'Contrato iniciado', 'success');
                verDetalle(contratoActual.contrato.idContrato);
                cargarContratos();
            } else {
                Swal.fire('Error', response.message, 'error');
            }
        } catch (error) {
            Swal.fire('Error', 'Error al iniciar contrato', 'error');
        }
    }
}
